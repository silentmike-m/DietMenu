<!--
<template>
  <v-card elevation="5">
    <v-card-text v-if="family">
      <v-text-field label="Nazwa" v-model="family.name" readonly></v-text-field>
      <v-btn color="primary" elevation="5" small @click="invite()"
        >Zaproś użytkowika</v-btn
      >
      <v-simple-table :class="{ mobile: !$vuetify.breakpoint.smAndUp }"
        ><template v-slot:default v-if="$vuetify.breakpoint.smAndUp">
          <thead>
            <tr>
              <th class="text-left">E-mail</th>
              <th class="text-left">Nazwa</th>
              <th class="text-left">Imię</th>
              <th class="text-left">Nazwisko</th>
              <th class="text-left">Status</th>
              <th class="text-left">Data zaproszenia</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in family.users" :key="user.id">
              <td>{{ user.email }}</td>
              <td>{{ user.user_name }}</td>
              <td>{{ user.first_name }}</td>
              <td>{{ user.last_name }}</td>
              <td>{{ user.status }}</td>
              <td>
                {{
                  user.invitation_date === undefined
                    ? ""
                    : new Date(user.invitation_date).toLocaleString("pl-PL")
                }}
              </td>
              <td>
                <v-btn
                  icon
                  v-if="user.status === 'Invited'"
                  @click="reInvite(user)"
                >
                  <v-icon small>mdi-refresh</v-icon>
                </v-btn>
              </td>
            </tr>
          </tbody></template
        >
        <template v-else v-slot:default
          ><tbody>
            <tr v-for="user in family.users" :key="user.id">
              <td>
                <ul class="flex-content">
                  <li class="flex-item" data-label="E-mail">
                    {{ user.email }}
                  </li>
                  <li class="flex-item" data-label="Nazwa">
                    {{ user.user_name }}
                  </li>
                  <li class="flex-item" data-label="Imię">
                    {{ user.first_name }}
                  </li>
                  <li class="flex-item" data-label="Nazwisko">
                    {{ user.last_name }}
                  </li>
                  <li class="flex-item" data-label="Status">
                    {{ user.status }}
                  </li>
                  <li class="flex-item" data-label="Data zaproszenia">
                    {{ user.invitation_date }}
                  </li>
                </ul>
              </td>
            </tr>
          </tbody></template
        ></v-simple-table
      >
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref } from "@nuxtjs/composition-api";
import useBreadcrumbs from "~/capi/core/useBreadcrumbs";
import useFamily from "~/capi/useFamily";
import { BreadcrumbType } from "~/types/core/breadcrumbs";
import useDialog from "~/capi/core/useDialog";
import { InputDialogOptions } from "~/types/core/dialog";
import { Guid } from "guid-typescript";
import useAlert from "~/capi/core/useAlert";
import { Family, FamilyUser } from "~/types/families";
import { Recipe } from "~/types/recipes";

export default Vue.extend({
  name: "Family",
  setup() {
    const family: Ref<Family | null> = ref(null);
    const recipe: Ref<Recipe> = ref({} as Recipe);

    const { showSuccess } = useAlert();
    const { setBreadcrumbs } = useBreadcrumbs();
    const { closeDialog, showInputDialog } = useDialog();
    const { getFamily, inviteUser } = useFamily();

    onMounted(() => {
      setBreadcrumbs(BreadcrumbType.Family, "");

      const familyId = "";
      getFamily(familyId).then((response) => {
        if (response) {
          family.value = response;
        }
      });
    });

    const invite = () => {
      showInputDialog(
        new InputDialogOptions(
          "Zaproszenie użytkownika",
          "email",
          (email: string) => {
            let user: FamilyUser = {
              id: Guid.create().toString(),
              first_name: "",
              email: email,
              last_name: "",
              invitation_date: new Date(),
              status: "Invited",
              user_name: "",
            };

            inviteUser(user).then((response) => {
              closeDialog();

              if (response) {
                family.value!.users.push(user);
                showSuccess("Zaproszenie zostało wysłane");
              }
            });
          },
          () => {
            closeDialog();
          }
        )
      );
    };

    const reInvite = (user: FamilyUser) => {
      inviteUser(user).then(() => {
        user.invitation_date = new Date();
        showSuccess("Zaproszenie zostało ponownie wysłane");
      });
    };
    return {
      family,
      invite,
      reInvite,
    };
  },
});
</script>

<style scoped>
@media screen and (max-width: 768px) {
  .mobile table tbody tr {
    max-width: 100%;
    display: block;
  }

  .mobile table tbody tr:nth-child(odd) {
    border-left: 6px solid #00aa95;
  }

  .mobile table tbody tr td {
    display: flex;
    width: 100%;
    border-bottom: 1px solid #f5f5f5 !important;
    height: auto !important;
    padding: 10px !important;
  }

  .mobile table tbody tr td ul li:before {
    content: attr(data-label);
    padding-right: 0.5em;
    text-align: left;
    display: block;
    color: #999;
  }
  .mobile table tbody tr td ul li .v-input--selection-controls {
    margin-top: 0;
  }
}

table tbody tr:nth-child(odd) {
  background: #f8f8f8;
}

.flex-content {
  padding: 0;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  width: 100%;
}

.flex-item {
  padding: 5px;
  width: 100%;
  font-weight: bold;
}

.action-column {
  width: 110px;
  text-align: center !important;
}
</style>
-->