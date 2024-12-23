import OpenAI from 'openai';
import type { ChatCompletionChunk, ChatCompletionCreateParamsStreaming } from 'openai/resources/chat/completions.mjs';
import { catchError, Observable, of, retry, switchMap,  } from 'rxjs';
import { fromFetch } from 'rxjs/fetch'; 


interface IMessage {
    Message : string;
}

//const client = new OpenAI({ apiKey: "-"});

export const chatService = {
  ask(question: string): Observable<ChatCompletionChunk> {

    var query = fromFetch("https://localhost:34557/chat", {
        body: JSON.stringify({ Message: question }),
        headers: {
            "Content-Type": "application/json",
        },
        method: "POST"
    });

    var response = query.pipe(switchMap(response => {
        if (response.ok) {
          // OK return data
          return response.json();
        } else {
          // Server is returning a status requiring the client to try something else.
          return of({ error: true, message: `Error ${ response.status }` });
        }
      }),
      catchError(err => {
        // Network or other error, handle appropriately
        console.error(err);
        return of({ error: true, message: err.message })
      }));

    return response; 
  }
};
