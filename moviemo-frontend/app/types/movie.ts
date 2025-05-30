export interface Movie {
    id: number
    title: string;
    overview: string;
    posterPath: string;
    trailerUrl: string;
}

export interface MovieCreateDto {
    title: string;
    overview: string;
    posterPath: string;
    trailerUrl: string;
}