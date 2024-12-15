<script lang="ts">
	import { page } from '$app/stores';
</script>

<header>
	<nav class="w-full flex">

		<ul class="w-full flex gap-4 justify-center bg-gray-900 text-sm font-medium text-white">
			<li class=""
				aria-current={$page.url.pathname === '/swap' ? 'page' : undefined}>
				<a class="rounded-md px-2 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white" href="/home">Dashboard</a>
			</li>
			<li class=""
				aria-current={$page.url.pathname === '/lights' ? 'page' : undefined}>
				<a class="rounded-md px-2 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white" href="/home">Lights</a>
			</li>
			<li class=""
				aria-current={$page.url.pathname === '/security' ? 'page' : undefined}>
				<a class="rounded-md px-2 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white" href="/home">Security</a>
			</li>
			<li class=""
			aria-current={$page.url.pathname === '/chat' ? 'page' : undefined}>
			<a class="rounded-md px-2 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white" href="/chat">Chat</a>
		</li>
		</ul>

	</nav>
</header>
