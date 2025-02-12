<script lang="ts">
	import { page } from '$app/stores';
	import { Icon, Bars3, Home, LightBulb, ShieldCheck, ChatBubbleLeft } from 'svelte-hero-icons';

	const navItems = [
		{ href: '/home', label: 'Dashboard', path: '/home', icon: Home },
		{ href: '/lights', label: 'Lights', path: '/lights', icon: LightBulb },
		{ href: '/security', label: 'Security', path: '/security', icon: ShieldCheck },
		{ href: '/chat', label: 'Chat', path: '/chat', icon: ChatBubbleLeft }
	];

	let isMenuOpen = false;
</script>

<header class="sticky top-0 z-50 w-full border-b border-gray-800 bg-gray-900/95 backdrop-blur supports-[backdrop-filter]:bg-gray-900/75">
	<nav class="container mx-auto px-4">
		<div class="flex items-center justify-between lg:hidden py-2">
			<button 
				on:click={() => isMenuOpen = !isMenuOpen}
				class="text-gray-400 hover:text-white p-2"
				aria-label="Toggle menu"
			>
				<Icon src={Bars3} class="h-6 w-6" />
			</button>
			<span class="text-white font-semibold">Simples</span>
		</div>

		<ul class="hidden lg:flex items-center justify-center space-x-2 py-2">
			{#each navItems as item}
				<li>
					<a
						href={item.href}
						class="group relative flex items-center gap-2 px-4 py-2 text-sm transition-colors rounded-md
							{$page.url.pathname === item.path
								? 'text-white font-medium bg-primary/10'
								: 'text-gray-400 hover:text-white hover:bg-gray-800'
							}"
						aria-current={$page.url.pathname === item.path ? 'page' : undefined}
					>
						<Icon 
							src={item.icon} 
							class="h-5 w-5 transition-colors
								{$page.url.pathname === item.path
									? 'text-primary'
									: 'text-gray-400 group-hover:text-white'
								}"
						/>
						<span>{item.label}</span>
						{#if $page.url.pathname === item.path}
							<span class="absolute bottom-0 left-0 h-[2px] w-full bg-primary"></span>
						{/if}
					</a>
				</li>
			{/each}
		</ul>

		<!-- Mobile menu -->
		{#if isMenuOpen}
			<ul class="lg:hidden py-2 space-y-1">
				{#each navItems as item}
					<li>
						<a
							href={item.href}
							class="flex items-center gap-3 px-4 py-3 text-sm transition-colors rounded-md
								{$page.url.pathname === item.path
									? 'text-white font-medium bg-primary/10'
									: 'text-gray-400 hover:text-white hover:bg-gray-800'
								}"
							aria-current={$page.url.pathname === item.path ? 'page' : undefined}
							on:click={() => isMenuOpen = false}
						>
							<Icon 
								src={item.icon} 
								class="h-5 w-5 {$page.url.pathname === item.path ? 'text-primary' : ''}"
							/>
							{item.label}
						</a>
					</li>
				{/each}
			</ul>
		{/if}
	</nav>
</header>
