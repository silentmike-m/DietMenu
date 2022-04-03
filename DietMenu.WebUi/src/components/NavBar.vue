<template>
  <nav class="navbar navbar-expand-md bg-primary navbar-dark">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/">
        <img src="@/static/logo32x32.png" alt="Dietkowanie" />
        Diet Plan
      </router-link>
      <button
        class="navbar-toggler"
        type="button"
        id="navBarCollapseButton"
        data-bs-toggle="collapse"
        data-bs-target="#navBarCollapse"
        aria-controls="navBarCollapse"
        aria-expanded="false"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navBarCollapse">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link
              class="btn btn-primary nav-link"
              to="/plans"
              @click="handleCollapse()"
              >Plan</router-link
            >
          </li>
          <li class="nav-item">
            <router-link
              class="btn btn-primary nav-link"
              to="/recipes"
              @click="handleCollapse()"
              >Przepisy</router-link
            >
          </li>
          <li class="nav-item">
            <router-link
              class="btn btn-primary nav-link"
              to="/replacements"
              @click="handleCollapse()"
              >Zamienniki</router-link
            >
          </li>
          <li class="nav-item">
            <router-link
              class="btn btn-primary nav-link"
              to="/ingredients"
              @click="handleCollapse()"
              >Składniki</router-link
            >
          </li>
          <li class="nav-item dropdown">
            <a
              class="btn btn-primary dropdown-toggle nav-link"
              href="#"
              id="navbarDropdown"
              role="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              Konfiguracja
            </a>
            <ul
              class="dropdown-menu bg-primary"
              aria-labelledby="navbarDropdown"
            >
              <li>
                <router-link
                  class="btn btn-primary dropdown-item"
                  to="/family"
                  @click="handleCollapse()"
                  >Rodzina</router-link
                >
              </li>
              <li>
                <router-link
                  class="btn btn-primary dropdown-item"
                  to="/ingredient-types"
                  @click="handleCollapse()"
                  >Rodzaje posiłków</router-link
                >
              </li>
              <li>
                <router-link
                  class="dropdown-item"
                  to="/meal-types"
                  @click="handleCollapse()"
                  >Rodzaje składników</router-link
                >
              </li>
            </ul>
          </li>
        </ul>
        <button class="btn btn-primary">{{ isMobile }}</button>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { inject, ComputedRef, onMounted } from "vue";
import AuthService from "@/services/AuthService";

export default {
  setup() {
    const isMobile = inject("isMobile") as ComputedRef<any>;

    const { getInformationAboutMySelf } = AuthService();

    onMounted(() => {
      getInformationAboutMySelf().then((response) => console.log(response));
    });

    const handleCollapse = () => {
      if (isMobile.value) {
        var navBarCollapseButton = document.getElementById(
          "navBarCollapseButton"
        );
        navBarCollapseButton?.click();
      }
    };

    return {
      isMobile,
      handleCollapse,
    };
  },
};
</script>

<style scoped>
.btn-primary:focus {
  background-color: transparent;
  border-color: transparent;
  box-shadow: none;
  outline: none;
}

.btn-primary:hover {
  background-color: #a5e887;
  color: #2e2e2e !important;
}

.dropdown-item {
  color: #2e2e2e !important;
}

.dropdown a:hover {
  background-color: #a5e887;
  color: #2e2e2e !important;
}

.nav-link {
  color: #2e2e2e !important;
}

.router-link-exact-active {
  color: #a5e887 !important;
}
</style>