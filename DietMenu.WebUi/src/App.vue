<template>
  <NavBar />
  <router-view />
  <DialogComponent />
  <AlertComponent />
</template>

<script lang="ts">
import { computed } from "vue";
import { Options, Vue } from "vue-class-component";
import AlertComponent from "@/components/AlertComponent.vue";
import DialogComponent from "@/components/dialog/DialogComponent.vue";
import NavBar from "@/components/NavBar.vue";

@Options({
  components: {
    AlertComponent,
    DialogComponent,
    NavBar,
  },
  data() {
    return {
      isMobile: window.innerWidth < 768,
    };
  },
  provide() {
    return {
      isMobile: computed(() => this.isMobile),
    };
  },
  created() {
    addEventListener("resize", () => {
      this.isMobile = innerWidth < 768;
    });
  },
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "@/assets/styles/style.scss";
</style>
