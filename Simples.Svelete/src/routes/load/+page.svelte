<svelte:head>
	<title>Home</title>
	<meta name="description" content="Test" />
</svelte:head>


<script>
    const props = {
        greeting: 'Hello',
        name: 'world'
    }

    let template = '<h1> {greeting} {name} </h1>'
    
    let filledTemplate = Object.entries(props).reduce((template, [key,value]) => {
        return template.replaceAll(`{${key}}`, value)
    },template)
            
</script>

{@html filledTemplate}