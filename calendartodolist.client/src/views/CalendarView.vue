<script setup lang="ts">
import ToggleButton from '@/components/calendar/ToggleButton.vue';
import WelcomeAlert from '@/components/calendar/WelcomeAlert.vue';
import LogoBanner from '@/components/LogoBanner.vue';
import { key } from '@/stores/auth';
import type { IAssignment } from '@/types/IAssignment';
import { deleteAssignment } from '@/utils/api/assignments/deleteAssignment';
import { getAssignments } from '@/utils/api/assignments/getAssignmentsMonth';
import { getAssignmentsPending } from '@/utils/api/assignments/getAssignmentsToday';
import { patchAssignment } from '@/utils/api/assignments/patchAssignment';
import { postAssignment } from '@/utils/api/assignments/postAssignment';
import { putAssignment } from '@/utils/api/assignments/putAssignment';
import d from '@/utils/date';
import { dateToJsonDateOnly } from '@/utils/parsers/dateToJsonDateOnly';
import { unrefObject } from '@/utils/parsers/unrefObject';
import { dateOnlyRules } from '@/utils/rules/dateOnlyRules';
import { requiredRule } from '@/utils/rules/requiredRule';
import { computed, onBeforeMount, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useStore } from 'vuex';


const chipsData = [
    { name: 'Completed', value: 'completed', color: 'green-darken-4', icon: 'mdi-check-circle' },
    { name: 'Not completed', value: 'not_completed', color: 'orange-darken-3', icon: 'mdi-close-circle' }
]

const store = useStore(key)
const router = useRouter()
const route = useRoute()

const months = ref([`${d.today().getFullYear()}-${d.today().getMonth() + 1}`])
const date = ref<string | undefined>(dateToJsonDateOnly(d.today()))
const chips = ref<string[]>([])
const edit = ref<IAssignment>()
const loading = ref(false)
const newAssignmentName = ref('')

const welcomeAlert = ref(route.query.status == 'new')
const text = ref(dateToJsonDateOnly(d.today()))

const allAssignments = ref<IAssignment[]>([])

onBeforeMount(async () => {
    if (!store.getters.isAuthorized)
        router.push('/')

    const today = new Date()
    loading.value = true
    const results = await Promise.all([
        getAssignments(today.getFullYear(), today.getMonth() + 1),
        getAssignmentsPending()
    ])
    if (results[0].isSuccess)
        allAssignments.value = results[0].assignments
    if (results[1].isSuccess) {
        allAssignments.value = results[1].assignments.reduce((acc, curr) => {
            const isDuplicated = acc.findIndex(a => a.id == curr.id) != -1
            if (!isDuplicated)
                acc.push(curr)
            return acc
        }, allAssignments.value)
    }
    loading.value = false
})

const handleChip = (type: string) => {
    const index = chips.value.indexOf(type);
    if (index > -1) {
        chips.value.splice(index, 1);
        return
    }

    chips.value.push(type)
}

const handleCalendar = async (v: any) => {
    const vDate = new Date(v)
    const os = vDate.getTimezoneOffset();
    const dateToParse = new Date(vDate.getTime() - os * 60 * 1000);

    const vYear = vDate.getFullYear()
    const vMonth = vDate.getMonth() + 1
    const isMonthFetched = months.value.includes(`${vYear}-${vMonth}`)
    if (!isMonthFetched) {
        loading.value = true
        const result = await getAssignments(vYear, vMonth)
        if (result.isSuccess) {
            allAssignments.value = result.assignments.reduce((acc, curr) => {
                const isDuplicated = acc.findIndex(a => a.id == curr.id) != -1
                if (!isDuplicated) {
                    acc.push(curr)
                }
                return acc
            }, allAssignments.value)
        }
        loading.value = false
    }

    date.value = dateToJsonDateOnly(dateToParse)
    text.value = dateToJsonDateOnly(dateToParse)
}

const handleEdit = (id: string) => {
    edit.value = unrefObject(assignments.value.find(a => a.id == id))
}

const handlePostAssignment = async () => {
    if (!date.value || !newAssignmentName.value)
        return

    loading.value = true
    const result = await postAssignment(newAssignmentName.value, date.value)
    if (result.isSuccess) {
        newAssignmentName.value = ''
        allAssignments.value.push(result.assignment)
    }
    loading.value = false
}

const handleCheck = async (assignment: IAssignment) => {
    loading.value = true
    const res = await patchAssignment(assignment.id, !assignment.isComplete)
    if (res.isSuccess)
        assignment.isComplete = !assignment.isComplete
    loading.value = false
}

const handleDelete = async (assignment: IAssignment) => {
    loading.value = true
    const res = await deleteAssignment(assignment.id)
    if (res.isSuccess) {
        const idx = allAssignments.value.indexOf(assignment)
        allAssignments.value.splice(idx, 1)
    }
    loading.value = false
}

const handleSaveEdit = async () => {
    if (edit.value == undefined)
        return

    loading.value = true
    const result = await putAssignment(edit.value)
    if (result.isSuccess) {
        const idx = assignments.value.findIndex(a => a.id = edit.value!.id)
        if (idx !== -1) {
            const edited = assignments.value[idx]
            edited.title = edit.value.title
            edited.date = edit.value.date
            edited.isComplete = edit.value.isComplete
        }
        edit.value = undefined
    }

    loading.value = false
}

const dateAmount = (date: string) => allAssignments.value.reduce((acc, curr) => {
    if (curr.date == date && !curr.isComplete)
        return acc + 1
    return acc
}, 0)

const todayAmount = computed(() => dateAmount(dateToJsonDateOnly(d.today())))
const tomorrowAmount = computed(() => {
    const tomorrow = new Date()
    tomorrow.setDate(tomorrow.getDate() + 1)
    return dateAmount(tomorrow.toJSON().slice(0, 10))
})

const pending = computed(() => {
    const todayDate = new Date(dateToJsonDateOnly(d.today()))
    return allAssignments.value.filter(a => !a.isComplete && new Date(a.date) < todayDate)
})

const assignments = computed<IAssignment[]>(() => {
    if (!date.value && !text.value)
        return []

    let proceeded: IAssignment[] = allAssignments.value

    if (date.value === undefined)
        proceeded = pending.value;
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

const pickerDate = computed(() => (date.value ? new Date(date.value) : undefined))

watch(text, value => date.value = (value == 'pending' ? undefined : value))
</script>

<template>
    <v-container v-if="store.getters.isAuthorized" class="h-100 justify-center d-flex flex-column">
        <v-card class="d-flex flex-column justify-center">
            <div class="justify-center text-center mt-4 mb-3">
                <LogoBanner />
                <v-card-subtitle>Plan your better tomorrow.</v-card-subtitle>
            </div>

            <WelcomeAlert v-model="welcomeAlert" />

            <ToggleButton v-model="text" :pending-count="pending.length" :today-count="todayAmount"
                :tomorrow-count="tomorrowAmount" />

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

            <div class="d-flex flex-row pa-2 flex-wrap">

                <v-date-picker show-adjacent-months class="flex-grow-1 border-t rounded-0 w-50" :disabled="loading"
                    @update:model-value="handleCalendar" :model-value="pickerDate"></v-date-picker>


                <div class="flex-grow-1 border-t rounded-0" style="min-width:50%;">
                    <div class="text-h5 pl-2 pt-6">Tasks:</div>
                    <div v-if="assignments.length < 1" style="min-height: 344px;"
                        class="w-100 d-flex text-center justify-center flex-column text-medium-emphasis font-weight-light">
                        <div>
                            No assignments
                        </div>
                        <div style="min-width: 300px;"></div>
                    </div>
                    <v-virtual-scroll v-else height="344" :items="assignments">
                        <template v-slot:default="{ item }">
                            <v-list-item>
                                <template v-slot:prepend>
                                    <v-list-item-action start>
                                        <v-checkbox-btn :disabled="loading" @update:model-value="handleCheck(item)"
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
                                        <v-btn color="blue-grey-lighten-1" icon="mdi-pen" variant="text"
                                            :disabled="loading" @click="handleEdit(item.id)"></v-btn>
                                    </v-list-item-action>
                                    <v-list-item-action end>
                                        <v-btn color="red-lighten-1" icon="mdi-delete" variant="text"
                                            :disabled="loading" @click="handleDelete(item)"></v-btn>
                                    </v-list-item-action>
                                </template>
                            </v-list-item>
                        </template>
                    </v-virtual-scroll>
                    <v-text-field v-model="newAssignmentName" class="ml-1 mr-2" append-icon="mdi-send"
                        clear-icon="mdi-close-circle" label="Task" type="text" variant="plain" clearable
                        :disabled="!date" @click:append="handlePostAssignment"
                        @click:clear="() => newAssignmentName = ''"></v-text-field>
                </div>
            </div>
        </v-card>
    </v-container>

    <v-overlay :model-value="!!edit" @click:outside="edit = undefined" class="align-center justify-center">

        <v-card v-if="edit" class="mx-auto pa-8 pb-8" elevation="8" max-width="448" min-width="360" rounded="lg"
            title="Edit Task">

            <div class="text-subtitle-1 text-medium-emphasis">Task name</div>
            <v-text-field density="compact" placeholder="Enter task name here" :rules="[requiredRule]"
                :disabled="loading" variant="outlined" v-model="edit.title"></v-text-field>

            <div class="text-subtitle-1 text-medium-emphasis">Date</div>
            <v-text-field density="compact" placeholder="YYYY-mm-dd" :rules="dateOnlyRules" :disabled="loading"
                variant="outlined" v-model="edit.date"></v-text-field>

            <v-checkbox label="Is Completed" :disabled="loading" v-model="edit.isComplete"></v-checkbox>

            <v-btn class="mb-4 text-none" color="blue-darken-1" size="large" variant="flat" @click="handleSaveEdit"
                :loading="loading" block>
                Save
            </v-btn>

        </v-card>

    </v-overlay>
</template>
