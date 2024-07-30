<script setup lang="ts">
const chips = defineModel<string[]>({ required: true })

type Chip = {
    name: string
    value: 'completed' | 'not_completed'
    color: string
    icon: string
}

const chipsData: Chip[] = [
    { name: 'Completed', value: 'completed', color: 'green-darken-4', icon: 'mdi-check-circle' },
    { name: 'Not completed', value: 'not_completed', color: 'orange-darken-3', icon: 'mdi-close-circle' }
]


const handleChip = (type: Chip['value']) => {
    const index = chips.value.indexOf(type);
    if (index > -1) {
        chips.value.splice(index, 1);
        return
    }

    chips.value.push(type)
}
</script>

<template>
    <div class="pa-1 mt-1">
        <span class="ml-2">
            Status:
        </span>
        <v-chip v-for="(data, idx) in chipsData" :key="idx" class="ma-1" :color="data.color"
            :variant="chips.includes(data.value) ? 'flat' : 'outlined'" @click="handleChip(data.value)">
            <v-icon :icon="data.icon" start></v-icon>
            {{ data.name }}
        </v-chip>
    </div>
</template>