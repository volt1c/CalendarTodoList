<script setup lang="ts">
import type { IAssignment } from '@/types/IAssignment';

type Method = (item: IAssignment) => void
type AsyncMethod = (item: IAssignment) => Promise<void>

defineProps<{
    items: IAssignment[],
    onCheck: Method | AsyncMethod,
    onEdit: Method | AsyncMethod,
    onDelete: Method | AsyncMethod,
    disabled?: boolean
}>()
</script>

<template>
    <v-virtual-scroll :items="items">
        <template v-slot:default="{ item }">
            <v-list-item>
                <template v-slot:prepend>
                    <v-list-item-action start>
                        <v-checkbox-btn :disabled="disabled" @update:model-value="onCheck(item)"
                            :model-value="item.isComplete"></v-checkbox-btn>
                    </v-list-item-action>
                </template>

                <v-list-item-title :color="item.isComplete && 'grey-darken-3'">
                    {{ item.title }}
                </v-list-item-title>

                <v-list-item-subtitle>
                    {{ item.date }}
                </v-list-item-subtitle>

                <template v-slot:append>
                    <v-list-item-action end>
                        <v-btn color="blue-grey-lighten-1" icon="mdi-pen" variant="text" :disabled="disabled"
                            @click="onEdit(item)"></v-btn>
                    </v-list-item-action>
                    <v-list-item-action end>
                        <v-btn color="red-lighten-1" icon="mdi-delete" variant="text" :disabled="disabled"
                            @click="onDelete(item)"></v-btn>
                    </v-list-item-action>
                </template>
            </v-list-item>
        </template>
    </v-virtual-scroll>

</template>