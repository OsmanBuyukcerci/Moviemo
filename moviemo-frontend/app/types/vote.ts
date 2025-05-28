export interface Vote {
    voteType: VoteType;
    commentId: number;
}

export enum VoteType {
    downvote = -1,
    upvote = 1
}