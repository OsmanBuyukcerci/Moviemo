export interface Comment {
    id: number;
    body: string;
    userId: number;
    username: string;
    movieId: number;
    createdAt: Date;
    updatedAt: Date;
    downvoteCounter: number;
    upvoteCounter: number;
}