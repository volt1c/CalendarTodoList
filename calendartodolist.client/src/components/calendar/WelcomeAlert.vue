<script setup lang="ts">
const model = defineModel({ default: true })
</script>

<template>
    <v-alert v-model="model" border="start" close-label="Close Alert" color="deep-purple-accent-4" class="mx-2"
        title="Welcome 🎉" variant="tonal" closable>
        We are glad that you decided to use our solution. We will take care of it to make using Calendar more
        and more convenient over time.
    </v-alert>
</template>