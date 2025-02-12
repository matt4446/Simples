export interface Message {
    text: string;
    sender: 'user' | 'bot';
    timestamp: string | null;
    isLoading: boolean;
    error?: boolean;
}