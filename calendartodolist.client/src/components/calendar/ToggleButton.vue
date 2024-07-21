<script setup lang="ts">
import date from '@/utils/date';
import { dateToJsonDateOnly } from '@/utils/parsers/dateToJsonDateOnly';

const today = dateToJsonDateOnly(date.today())
const tomorrow = dateToJsonDateOnly(date.tomorrow())

const model = defineModel()
defineProps<{
    pendingCount: number,
    todayCount: number,
    tomorrowCount: number,
}>()
</script>

<template>
    <v-btn-toggle v-model="model" color="deep-purple-accent-3" rounded="0" group
        class="w-100 d-flex border-t border-b mt-2">
        <v-btn color="yellow-darken-4" value="pending" class="flex-grow-1"> Pending: {{ pendingCount }}
        </v-btn>
        <v-btn color="green" :value="today" class="flex-grow-1"> Today: {{ tomorrowCount }} </v-btn>
        <v-btn color="blue" :value="tomorrow" class="flex-grow-1"> Tomorrow: {{ tomorrowCount }} </v-btn>
    </v-btn-toggle>
</template>