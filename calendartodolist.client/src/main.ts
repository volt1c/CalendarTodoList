import { createApp } from "vue";

import vuetify from "@/vuetify";
import store, { key } from "@/stores/auth";
import router from "@/router";
import App from "@/App.vue";

createApp(App).use(vuetify).use(router).use(store, key).mount("#app");
