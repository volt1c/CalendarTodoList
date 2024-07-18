<script setup lang="ts">
import LogoBanner from '@/components/LogoBanner.vue';
import type { IAssignment } from '@/types/IAssignment';
import { getAssignments } from '@/utils/api/assignments/getAssignmentsMonth';
import { getAssignmentsPending } from '@/utils/api/assignments/getAssignmentsToday';
import { today } from '@/utils/date/today';
import { tomorrow } from '@/utils/date/tomorrow';
import { dateToJsonDateOnly } from '@/utils/parsers/dateToJsonDateOnly';
import { computed, onBeforeMount, ref } from 'vue';
import { useRoute } from 'vue-router';

const chipsData = [
    { name: 'Completed', value: 'completed', color: 'green-darken-4', icon: 'mdi-check-circle' },
    { name: 'Not completed', value: 'not_completed', color: 'orange-darken-3', icon: 'mdi-close-circle' }
]


const route = useRoute()

const date = ref<string>(dateToJsonDateOnly(today()))
const chips = ref<string[]>([])

const welcomeAlert = ref(route.query.status == 'new')
const text = ref('today')

const allPending = ref<IAssignment[]>([])
const allAssignments = ref<IAssignment[]>([])

onBeforeMount(async () => {
    const today = new Date()
    const results = await Promise.all([
        getAssignments(today.getFullYear(), today.getMonth() + 1),
        getAssignmentsPending()
    ])
    if (results[0].isSuccess)
        allAssignments.value = results[0].assignments
    if (results[1].isSuccess)
        allPending.value = results[1].assignments
})

const handleToggle = () => {
    switch (text.value) {
        case 'pending':
            date.value = 'pending'
            return
        case 'today':
            date.value = dateToJsonDateOnly()
            return
        case 'tomorrow':
            date.value = dateToJsonDateOnly(tomorrow())
            return
        default:
            date.value = ''
            return
    }
}

const handleChip = (type: string) => {
    const index = chips.value.indexOf(type);
    if (index > -1) {
        chips.value.splice(index, 1);
        return
    }

    chips.value.push(type)
}

const handleCalendar = (v: any) => {
    const vDate = new Date(v)
    const os = vDate.getTimezoneOffset();
    const dateToParse = new Date(vDate.getTime() - os * 60 * 1000);
    date.value = dateToJsonDateOnly(dateToParse)
}

const dateAmount = (date: string) => allAssignments.value.reduce((acc, curr) => {
    if (curr.date == date)
        return acc + 1
    return acc
}, 0)

const todayAmount = computed(() => dateAmount(dateToJsonDateOnly(today())))
const tomorrowAmount = computed(() => {
    const tomorrow = new Date()
    tomorrow.setDate(tomorrow.getDate() + 1)
    return dateAmount(tomorrow.toJSON().slice(0, 10))
})

const assignments = computed<IAssignment[]>(() => {
    if (!date.value)
        return []

    let proceeded: IAssignment[] = allAssignments.value

    if (text.value === 'pending')
        proceeded = allPending.value;
    else if (date.value)
        proceeded = proceeded.filter(el => el.date == date.value)



    if (chips.value.length > 0)
        proceeded = proceeded.filter(el => {
            if (chips.value.includes('not_completed') && !el.isComplete)
                return true
            if (chips.value.includes('completed') && el.isComplete)
                return true
            return false
        })

    return proceeded
})
</script>

<template>
    <v-container class="h-100 justify-center d-flex flex-column">
        <v-card class="d-flex flex-column justify-center">
            <div class="justify-center text-center mt-4 mb-3">
                <LogoBanner />
                <v-card-subtitle>Plan your better tomorrow.</v-card-subtitle>
            </div>
            <v-alert v-model="welcomeAlert" border="start" close-label="Close Alert" color="deep-purple-accent-4"
                class="mx-2" title="Welcome 🎉" variant="tonal" closable>
                We are glad that you decided to use our solution. We will take care of it to make using Calendar more
                and more convenient over time.
            </v-alert>
            <v-btn-toggle v-model="text" color="deep-purple-accent-3" rounded="0" group @click="handleToggle"
                class="w-100 d-flex border-t border-b mt-2">
                <v-btn color="yellow-darken-4" value="pending" class="flex-grow-1"> Pending: {{ allPending.length }}
                </v-btn>
                <v-btn color="green" value="today" class="flex-grow-1"> Today: {{ todayAmount }} </v-btn>
                <v-btn color="blue" value="tomorrow" class="flex-grow-1"> Tomorrow:{{ tomorrowAmount }} </v-btn>
            </v-btn-toggle>
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
            <div class="d-flex flex-row pa-2">


                <v-date-picker show-adjacent-months class="flex-grow-1"
                    @update:model-value="handleCalendar"></v-date-picker>


                <div class="flex-grow-1">
                    <span class="text-h5">Tasks:</span>
                    <v-list lines="three" select-strategy="classic">
                        <v-list-item v-for="item in assignments" :key="item.id">
                            <template v-slot:prepend>
                                <v-list-item-action start>
                                    <v-checkbox-btn :model-value="item.isComplete"></v-checkbox-btn>
                                </v-list-item-action>
                            </template>

                            <v-list-item-title :color="item.isComplete && 'grey-darken-3'">
                                {{ item.title }}
                            </v-list-item-title>

                            <v-list-item-subtitle v-if="item.description">
                                {{ item.description }}
                            </v-list-item-subtitle>

                            <template v-slot:append>
                                <v-list-item-action end>
                                    <v-btn color="blue-grey-lighten-1" icon="mdi-pen" variant="text"></v-btn>
                                </v-list-item-action>
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
