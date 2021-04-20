<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="home">
    <h1 id="title" style="margin-top: 12px;">Profile</h1>

    <div id="profile-content">
      <h3>Name:</h3>
      <b-form-input id="usersname" v-model="usersname"></b-form-input>
      <h3>Email:</h3>
      <b-form-input v-model="email" id="usersemail" disabled></b-form-input>
      <h3>Password:</h3>
      <b-button variant="primary" @click="goToOktaProfile()">
        Change Password
      </b-button>
    </div>
    <!-- The following code can be re-added when the emergency contacts are linked to the API -->
    <!-- <div class="d-flex justify-content-center">
      <h3>Emergency Contacts</h3>
    </div> -->

    <!-- emit code based on tutorial https://vuejs.org/v2/guide/components.html-->
    <!-- <div class="d-flex flex-wrap" id="emergency-contacts">
      <div
        class="card"
        style="width: 17rem;"
        v-for="(contact, index) in contacts"
        :key="contact.name"
      >
        <Contact
          v-bind="contact"
          v-on:edit="edit(index)"
          v-on:delete="deleteContact(index)"
        ></Contact>
      </div>
    </div>

    <div class="d-flex justify-content-center">
      <b-button variant="primary" id="add_button" @click="add()"
        >Add A Contact</b-button
      >
    </div> -->

    <!--Add Contact modal-->
    <!--Code based on bootstrap tutorial Prevent closing Modal https://bootstrap-vue.org/docs/components/modal-->
    <!-- <b-modal
      id="popup"
      ref="modal"
      v-model="popupShow"
      :title="title"
      @ok="handleOk"
    >
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-form-group
          label="Name"
          label-for="name-input"
          invalid-feedback="Name is required"
          :state="nameState"
        >
          <b-form-input
            id="name-input"
            v-model="inputname"
            :state="nameState"
            required
          ></b-form-input>
        </b-form-group>
        <b-form-group
          label="Number"
          label-for="number-input"
          invalid-feedback="Phone number is required"
          :state="numberState"
        >
          <b-form-input
            id="number-input"
            v-model="inputnumber"
            :state="numberState"
            required
          ></b-form-input>
        </b-form-group>
      </form>
    </b-modal> -->
  </div>
</template>

<script>
// import Contact from '@/components/Contact.vue';

import { getContacts } from '@/utils/contactTools.js';
export default {
  name: 'Home',
  // components: {
  //   Contact,
  // },
  async created() {
    const user = await this.$auth.getUser();
    this.usersname = user.name;
    this.email = user.email;
    this.contacts = getContacts();
  },
  data: function() {
    return {
      contacts: [],
      title: '',
      inputname: '',
      popupShow: false,
      nameState: null,
      inputnumber: '',
      numberState: null,
      saveindex: 0,
      usersname: '',
      email: '',
    };
  },
  methods: {
    //Code based on bootstrap tutorial Prevent closing Modal https://bootstrap-vue.org/docs/components/modal
    checkFormValidity() {
      const valid = this.$refs.form.checkValidity();
      this.nameState = valid;
      this.numberState = valid;
      return valid;
    },
    goToOktaProfile() {
      window.location.href = 'https://dev-76404687.okta.com/enduser/settings/';
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    handleSubmit() {
      // Exit when the form isn't valid
      if (!this.checkFormValidity()) {
        return;
      }
      // add contact
      if (this.title === 'Edit Contact') {
        this.contacts.splice(this.saveIndex, 1);

        this.contacts.push({
          name: this.inputname,
          number: this.inputnumber,
        });
      } else {
        this.contacts.push({
          name: this.inputname,
          number: this.inputnumber,
        });
      }
      this.nameState = null;
      this.numberState = null;

      // Hide the modal manually
      this.popupShow = false;
    },
    edit(index) {
      this.title = 'Edit Contact';
      this.inputname = this.contacts[index].name;
      this.inputnumber = this.contacts[index].number;
      this.saveIndex = index;

      this.popupShow = true;
    },
    add: function() {
      this.title = 'Add Contact';
      this.inputname = '';
      this.inputnumber = '';

      this.popupShow = true;
    },
    deleteContact(index) {
      this.contacts.splice(index, 1);
    },
  },
};
</script>

<style>
#title {
  font-family: Helvetica, Arial, sans-serif;
  text-align: center;
}
#profile-content {
  position: relative;
  left: 50%;
  transform: translateX(-50%);
  max-width: 502px;
}
#profile-content h3 {
  font-family: Helvetica, Arial, sans-serif;
  text-align: left;
  margin-top: 50px;
}
#profile-content input {
  margin-top: 10px;
  margin: 0 auto;
  max-width: 500px;
}
#profile-content button {
  margin-top: 10px;
}
#add_button {
  margin-bottom: 100px;
}
.contacts {
  margin-top: 20px;
  margin-bottom: 50px;
}
#contact_title {
  margin-right: 200px;
}
#emergency-contacts {
  margin-left: 50px;
  margin-bottom: 50px;
}
.card {
  margin-left: 50px;
  margin-top: 50px;
}
</style>
