<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
</script>

<template>
    <header>
        <RouterLink to="/"> Header </RouterLink> 
        <RouterLink to="/login"> Login </RouterLink>
        <RouterLink to="/register"> Register </RouterLink>
    </header>

  <main>
    <RouterView />
  </main>
</template>
