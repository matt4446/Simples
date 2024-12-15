import { sveltekit } from '@sveltejs/kit/vite';
import tailwindcss from '@tailwindcss/vite';
import path from "path";

import { defineConfig } from 'vite';

export default defineConfig({
	resolve: {
		alias: {
		  $lib: path.resolve("./src/lib"),
		},
	  },
	plugins: [tailwindcss(), sveltekit()]
});
