<template>
  <NavBar />
  <router-view />
  <DialogComponent />
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import DialogComponent from "@/components/dialog/DialogComponent.vue";
import NavBar from "@/components/NavBar.vue";

@Options({
  components: {
    DialogComponent,
    NavBar,
  },
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "@/assets/styles/style.scss";
</style>
