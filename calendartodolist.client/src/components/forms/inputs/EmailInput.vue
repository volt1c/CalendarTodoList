<script setup lang="ts">
import { emailRules } from '@/utils/rules/emilRules';

const model = defineModel({ default: '' })

defineProps<{
    disabled?: boolean
}>()
</script>

<template>
    <div class="text-subtitle-1 text-medium-emphasis">Email</div>

    <v-text-field density="compact" prepend-inner-icon="mdi-email-outline" variant="outlined"
        placeholder="Email address" :rules="emailRules" v-model="model" :disabled="disabled"></v-text-field>
</template>