export interface Message {
    text: string;
    sender: 'user' | 'bot';
    author: string;
    authorName: string;
    timestamp: string | null;
    isLoading: boolean;
    error?: boolean;
}