'use client';

import { useState, useEffect } from 'react';
import { useParams } from 'next/navigation';
import { Movie } from '@/app/types/movie';
import { Review } from '@/app/types/review';
import { Comment } from '@/app/types/comment';
import { apiService } from '@/app/services/api';
import { VoteType } from '@/app/types/vote';
import { Vote, Edit, Trash2 } from 'lucide-react';
import { getCurrentUserId } from '@/app/utils/user';

export default function MoviePage() {
  const { id } = useParams();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [comments, setComments] = useState<Comment[]>([]);
  const [newReview, setNewReview] = useState({ body: '', userScore: 0 });
  const [newComment, setNewComment] = useState('');
  const [editingComment, setEditingComment] = useState<{ id: number; body: string } | null>(null);
  const [editingReview, setEditingReview] = useState<{ id: number; body: string; userScore: number } | null>(null);
  const [loading, setLoading] = useState(true);
  const [currentUserId, setCurrentUserId] = useState<number | null>(-1);

  const apiUrl = `https://localhost:7179/api/movies/${id}`;
  const usersApiUrl = 'https://localhost:7179/api/users';
  const reviewsApiUrl = 'https://localhost:7179/api/reviews';
  const commentsApiUrl = 'https://localhost:7179/api/comments';
  const votesApiUrl = 'https://localhost:7179/api/votes';

  const getEmbedUrl = (url: string) => {
    if (!url) return '';
    const videoIdMatch = url.match(/(?:v=|\.be\/)([^&?]+)/);
    const videoId = videoIdMatch ? videoIdMatch[1] : '';
    return videoId ? `https://www.youtube.com/embed/${videoId}` : url;
  };

  const getVoteId = async (commentId: number) => {
    try {
      const response = await fetch(
        `${votesApiUrl}?userId=${currentUserId!}&commentId=${commentId}`,
        { method: 'GET', headers: apiService.getHeaders() }
      );

      const data = await response.json();
      return [data.id, data.voteType];
    } catch (err) {
      console.error('Vote GET error:', err);
      return [null, null];
    }
  };

  const handleVote = async (commentId: number, inputVoteType: number) => {
    if (!apiService.isAuthenticated()) {
      window.location.href = '/login';
    }

    const data = await getVoteId(commentId);
    const id: number = data[0];
    const voteType: number = data[1];

    let updatedComments = [...comments];

    const commentIndex = comments.findIndex(c => c.id === commentId);

    if ((id === -1 && voteType === -2) || (id == null && voteType == null)) {
      await fetch(votesApiUrl, {
        method: 'POST',
        body: JSON.stringify({ voteType: inputVoteType, commentId }),
        headers: apiService.getHeaders(true),
      });
      
      if (inputVoteType === VoteType.upvote) {
        updatedComments[commentIndex].upvoteCounter += 1;
      } else {
        updatedComments[commentIndex].downvoteCounter += 1;
      }
    } 
    else if (voteType === inputVoteType) {
      await fetch(`${votesApiUrl}/${id}`, {
        method: 'DELETE',
        headers: apiService.getHeaders(true),
      });
      if (inputVoteType == VoteType.upvote) {
        updatedComments[commentIndex].upvoteCounter -= 1;
      } else {
        updatedComments[commentIndex].downvoteCounter -= 1;
      }
    } else {
      await fetch(`${votesApiUrl}/${id}`, {
        method: 'PUT',
        body: JSON.stringify({ voteType: inputVoteType }),
        headers: apiService.getHeaders(true),
      });
      if (inputVoteType == VoteType.upvote) {
        updatedComments[commentIndex].upvoteCounter += 1;
        updatedComments[commentIndex].downvoteCounter -= 1;
      } else {
        updatedComments[commentIndex].upvoteCounter -= 1;
        updatedComments[commentIndex].downvoteCounter += 1;
      }
    }
    setComments(updatedComments);
  };

  const handleReviewSubmit = async (e: React.FormEvent) => {
    if (!apiService.isAuthenticated()) {
      window.location.href = '/login';
    }

    if (reviews.find(r => r.userId == currentUserId)) {
      alert("Birden fazla inceleme yapamazsınız.");
      return;
    }

    e.preventDefault();
    try {
      await fetch(reviewsApiUrl, {
        method: 'POST',
        body: JSON.stringify({ movieId: id, ...newReview }),
        headers: apiService.getHeaders(true),
      });
      const response = await fetch(`${reviewsApiUrl}?movieId=${id}`, {
        headers: apiService.getHeaders(),
      });
      const updatedReviews = await response.json();
      const reviewsWithUsername = await Promise.all(
        updatedReviews.map(async (review: Review) => {
          const username = await fetchUsername(review.userId);
          return { ...review, username };
        })
      );
      setReviews(reviewsWithUsername);
      setNewReview({ body: '', userScore: 0 });
    } catch (err) {
      console.error('Review submit error:', err);
    }
  };

  const handleCommentSubmit = async (e: React.FormEvent) => {
    if (!apiService.isAuthenticated()) {
      window.location.href = '/login';
    }

    e.preventDefault();
    try {
      await fetch(commentsApiUrl, {
        method: 'POST',
        body: JSON.stringify({ movieId: id, body: newComment }),
        headers: apiService.getHeaders(true),
      });
      const response = await fetch(`${commentsApiUrl}?movieId=${id}`, {
        headers: apiService.getHeaders(),
      });
      const updatedComments = await response.json();
      const commentsWithUsername = await Promise.all(
        updatedComments.map(async (comment: Comment) => {
          const username = await fetchUsername(comment.userId);
          return { ...comment, username };
        })
      );
      setComments(commentsWithUsername);
      setNewComment('');
    } catch (err) {
      console.error('Comment submit error:', err);
    }
  };

  const handleEditComment = async (commentId: number) => {
    if (editingComment) {
      try {
        await fetch(`${commentsApiUrl}/${commentId}`, {
          method: 'PUT',
          body: JSON.stringify({ body: editingComment.body }),
          headers: apiService.getHeaders(true),
        });
        const updatedComments = comments.map(comment =>
          comment.id === commentId ? { ...comment, body: editingComment.body } : comment
        );
        setComments(updatedComments);
        setEditingComment(null);
      } catch (err) {
        console.error('Comment edit error:', err);
      }
    }
  };

  const handleDeleteComment = async (commentId: number) => {
    try {
      await fetch(`${commentsApiUrl}/${commentId}`, {
        method: 'DELETE',
        headers: apiService.getHeaders(true),
      });
      setComments(comments.filter(comment => comment.id !== commentId));
    } catch (err) {
      console.error('Comment delete error:', err);
    }
  };

  const handleEditReview = async (reviewId: number) => {
    if (editingReview) {
      try {
        await fetch(`${reviewsApiUrl}/${reviewId}`, {
          method: 'PUT',
          body: JSON.stringify({ body: editingReview.body, userScore: editingReview.userScore }),
          headers: apiService.getHeaders(true),
        });
        const updatedReviews = reviews.map(review =>
          review.id === reviewId ? { ...review, body: editingReview.body, userScore: editingReview.userScore } : review
        );
        setReviews(updatedReviews);
        setEditingReview(null);
      } catch (err) {
        console.error('Review edit error:', err);
      }
    }
  };

  const handleDeleteReview = async (reviewId: number) => {
    try {
      await fetch(`${reviewsApiUrl}/${reviewId}`, {
        method: 'DELETE',
        headers: apiService.getHeaders(true),
      });
      setReviews(reviews.filter(review => review.id !== reviewId));
    } catch (err) {
      console.error('Review delete error:', err);
    }
  };

  const fetchUsername = async (userId: number) => {
    const response = await fetch(`${usersApiUrl}/${userId}`, {
      method: 'GET',
      headers: apiService.getHeaders(),
    });
    const { username } = await response.json();
    return username;
  };

  useEffect(() => {
    const fetchMovie = async () => {
      try {
        const response = await fetch(apiUrl, { headers: apiService.getHeaders() });
        const { id, title, overview, posterPath, trailerUrl } = await response.json();
        setMovie({ id, title, overview, posterPath, trailerUrl });
      } catch (err) {
        console.error('Fetch movie error:', err);
      }
    };

    const fetchReviews = async () => {
      try {
        const response = await fetch(`${reviewsApiUrl}?movieId=${id}`, {
          headers: apiService.getHeaders(),
        });
        const reviews = await response.json();
        const reviewsWithUsername = await Promise.all(
          reviews.map(async (review: Review) => {
            const username = await fetchUsername(review.userId);
            return { ...review, username };
          })
        );
        setReviews(reviewsWithUsername);
      } catch (err) {
        console.error('Fetch reviews error:', err);
      }
    };

    const fetchComments = async () => {
      try {
        const response = await fetch(`${commentsApiUrl}?movieId=${id}`, {
          headers: apiService.getHeaders(),
        });
        const comments = await response.json();
        const commentsWithUsername = await Promise.all(
          comments.map(async (comment: Comment) => {
            const username = await fetchUsername(comment.userId);
            return { ...comment, username };
          })
        );
        setComments(commentsWithUsername);
      } catch (err) {
        console.error('Fetch comments error:', err);
      }
    };

    const getAndSetCurrentUserId = async () => {
      try {
        const id = await getCurrentUserId();

        if (id != null) {
          setCurrentUserId(id);
        }
      } catch (err) {
        console.error('Fetch user ID error: ', err);
      }
    }

    const fetchDetails = async () => {
      await Promise.all([fetchMovie(), fetchReviews(), fetchComments(), getAndSetCurrentUserId()]);
      setLoading(false);
    };

    fetchDetails();
  }, [id]);

  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 flex items-center justify-center px-4">
        <div className="animate-spin rounded-full h-12 w-12 border-t-4 border-purple-500"></div>
      </div>
    );
  }

  return (
  <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 px-4 sm:px-6 lg:px-8 py-12">
    <div className="max-w-5xl mx-auto">
      {/* Movie Details */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-12">
        {/* Poster */}
        <div className="">
          <img
            src={movie?.posterPath}
            alt={movie?.title}
            className="w-full h-auto rounded-lg shadow-2xl border border-white/20"
          />
        </div>
        {/* Trailer and Overview */}
        <div className="flex flex-col space-y-6 h-full">
          <div className="flex-1 aspect-w-16 aspect-h-9">
            <iframe
              src={movie?.trailerUrl ? getEmbedUrl(movie.trailerUrl) : ''}
              title={movie?.title}
              className="w-full h-full rounded-lg shadow-2xl border border-white/20"
              allowFullScreen
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            />
          </div>
          <div className="flex-1 bg-white/10 backdrop-blur-lg rounded-2xl p-6 shadow-2xl border border-white/20">
            <h2 className="text-3xl font-extrabold text-white mb-4">{movie?.title}</h2>
            <p className="text-gray-300">{movie?.overview}</p>
          </div>
        </div>
      </div>

      {/* Reviews Section */}
      <div className="mb-12">
        <h2 className="text-2xl font-bold text-white mb-6">İncelemeler</h2>
        {/* Review Form */}
        <div className="bg-white/10 backdrop-blur-lg rounded-2xl p-6 shadow-2xl border border-white/20 mb-6">
          <form onSubmit={handleReviewSubmit} className="space-y-4">
            <div>
              <label htmlFor="review" className="block text-sm font-medium text-gray-200 mb-2">
                İncelemenizi Yazın
              </label>
              <textarea
                id="review"
                value={newReview.body}
                onChange={(e) => setNewReview({ ...newReview, body: e.target.value })}
                className="block w-full px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                placeholder="Filme dair düşüncelerinizi paylaşın"
                rows={4}
              />
            </div>
            <div>
              <label htmlFor="userScore" className="block text-sm font-medium text-gray-200 mb-2">
                Puanınız (0-10)
              </label>
              <input
                id="userScore"
                type="number"
                min="0"
                max="10"
                value={newReview.userScore}
                onChange={(e) => setNewReview({ ...newReview, userScore: parseInt(e.target.value) })}
                className="block w-24 px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
              />
            </div>
            <button
              type="submit"
              className="group relative flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 transition-all duration-200 transform hover:scale-105"
            >
              İnceleme Gönder
            </button>
          </form>
        </div>
        {/* Reviews List */}
        <div className="space-y-4">
          {reviews.map(review => (
            <div
              key={review.id}
              className="relative bg-white/10 backdrop-blur-lg rounded-lg p-4 shadow-2xl border border-white/20"
            >
              {editingReview && editingReview.id === review.id ? (
                <div className="space-y-4">
                  <textarea
                    value={editingReview.body}
                    onChange={(e) => setEditingReview({ ...editingReview, body: e.target.value })}
                    className="block w-full px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                    placeholder="İncelemenizi düzenleyin"
                    rows={4}
                  />
                  <input
                    type="number"
                    min="0"
                    max="10"
                    value={editingReview.userScore}
                    onChange={(e) => setEditingReview({ ...editingReview, userScore: parseInt(e.target.value) })}
                    className="block w-24 px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                  />
                  <div className="flex space-x-2">
                    <button
                      onClick={() => handleEditReview(review.id)}
                      className="group relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 transition-all duration-200"
                    >
                      Kaydet
                    </button>
                    <button
                      onClick={() => setEditingReview(null)}
                      className="group relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gray-600 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 transition-all duration-200"
                    >
                      İptal
                    </button>
                  </div>
                </div>
              ) : (
                <div className="space-y-3">
                  {/* Review Header - Date, Username and Action Buttons */}
                  <div className="flex justify-between items-start">
                    <div className="text-gray-300">
                      <span className="font-medium text-white">{new Date(review.createdAt).toLocaleDateString()}</span>{' '}
                      <span className="font-medium text-white">{review.username}</span>
                    </div>
                    {currentUserId === review.userId ? (
                      <div className="flex items-center space-x-2">
                        <button
                          onClick={() => setEditingReview({ id: review.id, body: review.body, userScore: review.userScore })}
                          className="text-gray-400 hover:text-white transition-colors p-1"
                          title="Düzenle"
                        >
                          <Edit className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleDeleteReview(review.id)}
                          className="text-gray-400 hover:text-red-400 transition-colors p-1"
                          title="Sil"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    ) : (
                      <div></div>
                    )}
                  </div>
                  {/* Review Body */}
                  <p className="text-gray-300 pr-4">
                    {review.body}
                  </p>
                  {/* Review Score */}
                  <div className="flex items-center justify-end">
                    <span
                      className={`w-8 h-8 rounded-full text-white text-sm font-medium flex items-center justify-center ${
                        review.userScore <= 4 ? 'bg-red-500' : review.userScore <= 7 ? 'bg-yellow-500' : 'bg-green-500'
                      }`}
                    >
                      {review.userScore}
                    </span>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>

      {/* Comments Section */}
      <div>
        <h2 className="text-2xl font-bold text-white mb-6">Yorumlar</h2>
        {/* Comment Form */}
        <div className="bg-white/10 backdrop-blur-lg rounded-2xl p-6 shadow-2xl border border-white/20 mb-6">
          <form onSubmit={handleCommentSubmit} className="space-y-4">
            <div>
              <label htmlFor="comment" className="block text-sm font-medium text-gray-200 mb-2">
                Yorumunuzu Yazın
              </label>
              <textarea
                id="comment"
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                className="block w-full px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                placeholder="Yorumunuzu paylaşın"
                rows={3}
              />
            </div>
            <button
              type="submit"
              className="group relative flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 transition-all duration-200 transform hover:scale-105"
            >
              Yorum Gönder
            </button>
          </form>
        </div>
        {/* Comments List */}
        <div className="space-y-4">
          {comments.map(comment => (
            <div
              key={comment.id}
              className="bg-white/10 backdrop-blur-lg rounded-lg p-4 shadow-2xl border border-white/20"
            >
              {editingComment && editingComment.id === comment.id ? (
                <div className="space-y-4">
                  <textarea
                    value={editingComment.body}
                    onChange={(e) => setEditingComment({ ...editingComment, body: e.target.value })}
                    className="block w-full px-3 py-3 border border-gray-600 rounded-lg bg-gray-800/50 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200"
                    rows={3}
                  />
                  <div className="flex space-x-2">
                    <button
                      onClick={() => handleEditComment(comment.id)}
                      className="group relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 transition-all duration-200"
                    >
                      Kaydet
                    </button>
                    <button
                      onClick={() => setEditingComment(null)}
                      className="group relative flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-lg text-white bg-gray-600 hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 transition-all duration-200"
                    >
                      İptal
                    </button>
                  </div>
                </div>
              ) : (
                <div className="space-y-3">
                  {/* Comment Header - Date, Username and Action Buttons */}
                  <div className="flex justify-between items-start">
                    <div className="text-gray-300">
                      <span className="font-medium text-white">{new Date(comment.createdAt).toLocaleDateString()}</span>{' '}
                      <span className="font-medium text-white">{comment.username}</span>
                    </div>
                    {currentUserId == comment.userId ? <div className="flex items-center space-x-2">
                      <button
                        onClick={() => setEditingComment({ id: comment.id, body: comment.body })}
                        className="text-gray-400 hover:text-white transition-colors p-1"
                        title="Düzenle"
                      >
                        <Edit className="h-4 w-4" />
                      </button>
                      <button
                        onClick={() => handleDeleteComment(comment.id)}
                        className="text-gray-400 hover:text-red-400 transition-colors p-1"
                        title="Sil"
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div> : <div></div>
                    }
                  </div>
                  
                  {/* Comment Body */}
                  <p className="text-gray-300 pr-4">
                    {comment.body}
                  </p>
                  
                  {/* Vote Buttons */}
                  <div className="flex items-center justify-end space-x-2 pt-2">
                    <button
                      onClick={() => handleVote(comment.id, VoteType.downvote)}
                      className="text-gray-400 hover:text-red-400 transition-colors p-1"
                      title="Beğenme"
                    >
                      <Vote className="h-4 w-4 rotate-180" />
                    </button>
                    <span className="text-white font-medium min-w-[20px] text-center">
                      {comment.upvoteCounter - comment.downvoteCounter}
                    </span>
                    <button
                      onClick={() => handleVote(comment.id, VoteType.upvote)}
                      className="text-gray-400 hover:text-green-400 transition-colors p-1"
                      title="Beğen"
                    >
                      <Vote className="h-4 w-4" />
                    </button>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
    </div>
  </div>
  );
}
