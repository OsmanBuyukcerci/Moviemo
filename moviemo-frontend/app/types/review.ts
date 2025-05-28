export interface Review {
    id: number;
    body: string;
    userId: number;
    username: string;
    movieId: number;
    userScore: number;
    createdAt: Date;
    updatedAt: Date;
}