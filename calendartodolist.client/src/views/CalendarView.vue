<script setup lang="ts">
import LogoBanner from '@/components/LogoBanner.vue';
import type { IAssignment } from '@/types/IAssignment';
import { ref, watch } from 'vue';

const assignments = ref<IAssignment[]>([
    {
        id: '1',
        date: '2024-07-18',
        title: 'Super Zadanie',
        description: '',
        isComplete: false
    }
])
const completeClass = {
    'text-decoration-line-through': true,
}
watch(assignments, () => console.log(assignments))
</script>

<template>
    <v-container class="h-100 justify-center d-flex flex-column">
        <v-card class="d-flex flex-column justify-center mt-2 pa-2">
            <div class="justify-center text-center mt-4 mb-7">
                <LogoBanner />
                <v-card-subtitle>Plan your better tomorrow.</v-card-subtitle>
            </div>
            <div class="d-flex flex-row">


                <v-date-picker show-adjacent-months class="flex-grow-1"></v-date-picker>


                <div class="flex-grow-1">
                    <span class="text-h5">Tasks:</span>
                    <v-list lines="three" select-strategy="classic">
                        <v-list-item v-for="item in assignments" :key="item.id">
                            <template v-slot:prepend>
                                <v-list-item-action start>
                                    <v-checkbox-btn :model-value="item.isComplete"></v-checkbox-btn>
                                </v-list-item-action>
                            </template>

                            <v-list-item-title :class="item.isComplete && completeClass"
                                :color="item.isComplete && 'grey-darken-3'">
                                {{ item.title }}
                            </v-list-item-title>

                            <v-list-item-subtitle v-if="item.description">
                                {{ item.description }}
                            </v-list-item-subtitle>

                            <template v-slot:append>
                                <v-list-item-action end>
                                    <v-btn color="red-lighten-1" icon="mdi-delete" variant="text"></v-btn>
                                </v-list-item-action>
                            </template>
                        </v-list-item>
                    </v-list>
                </div>
            </div>
        </v-card>
    </v-container>
</template>
