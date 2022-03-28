<template>
  <v-app :style="{ background: $vuetify.theme.themes['light'].background }">
    <v-navigation-drawer
      v-model="showMenu"
      clipped
      fixed
      app
      :mini-variant="isMiniVariant && $vuetify.breakpoint.smAndUp"
      color="primary"
    >
      <NavBar />
    </v-navigation-drawer>
    <v-app-bar
      :clipped-left="true"
      fixed
      app
      color="primary"
      class="white--text"
    >
      <img src="~/static/logo32x32.png" alt="Dietkowanie" />
      <v-app-bar-nav-icon
        @click.stop="handleMiniVariant()"
        class="white--text"
      />
      <v-toolbar-title></v-toolbar-title>
      <Breadcrumbs v-if="$vuetify.breakpoint.smAndUp" />
      <v-spacer />
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn color="primary" dark v-bind="attrs" v-on="on">
            {{ userName }}
          </v-btn>
        </template>
        <v-list color="primary">
          <v-list-item href="/auth/changePassword">
            <v-list-item-icon>
              <v-icon>mdi-lastpass</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Zmień hasło</v-list-item-title>
          </v-list-item>
          <v-list-item href="#" @click="logoutUser()">
            <v-list-item-icon>
              <v-icon>mdi-logout</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Wyloguj</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-main>
      <v-container fluid>
        <nuxt />
      </v-container>
    </v-main>
    <Alert />
    <DialogRoot />
  </v-app>
</template>

<style scoped>
.theme--light.v-icon {
  color: white !important;
}
</style>

<script lang="ts">
import Vue from "vue";
import Alert from "~/components/dialogs/alert.vue";
import DialogRoot from "~/components/dialogs/dialogRoot.vue";
import NavBar from "~/components/navBar.vue";
import Breadcrumbs from "~/components/breadcrumbs.vue";
import { onMounted, Ref, ref } from "@nuxtjs/composition-api";
import applicationBarState from "~/store/appBarStore";

export default Vue.extend({
  name: "empty",
  components: {
    Alert,
    DialogRoot,
    NavBar,
    Breadcrumbs,
  },
  setup() {
    const showMenu: Ref<Boolean> = ref(true);
    const userName: Ref<string> = ref("");

    // const { logOut } = useUsers();

    onMounted(() => {
      userName.value = "USERNAME";
    });

    const handleMiniVariant = (): void => {
      if (!showMenu.value) {
        showMenu.value = true;
      } else {
        applicationBarState.setMiniVariant();
      }
    };

    const logoutUser = (): void => {
      //logOut();
    };

    return {
      handleMiniVariant,
      logoutUser,
      showMenu,
      userName,
    };
  },
  computed: {
    isMiniVariant() {
      return applicationBarState.get();
    },
  },
});
</script>

<style>
.container {
  padding: 5px !important;
}
</style>
