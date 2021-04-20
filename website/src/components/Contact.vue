<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="card-body">
    <b-avatar id="contactavatar" size="50px"></b-avatar>
    <p class="card-text">Name: {{ name }}</p>
    <p class="card-text">Number: {{ number }}</p>
    <div>
      <font-awesome-icon
        icon="edit"
        id="editicon"
        size="2x"
        @click="editClicked"
      />
      <font-awesome-icon
        icon="trash"
        id="trashicon"
        size="2x"
        @click="deleteClicked"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: 'Contact',
  props: {
    name: String,
    number: String,
  },
  methods: {
    editClicked: function() {
      this.$emit('edit');
    },
    deleteClicked: function() {
      this.$emit('delete');
    },
    setHoveringStatus: function(value) {
      this.hovering = value;
    },
  },
};
</script>

<style>
#editicon {
  margin-right: 10px;
  color: Gray;
}
#editicon:hover {
  color: #4b9f00;
}
#trashicon {
  color: #ff6a33;
}
#trashicon:hover {
  color: #e64307;
}
#contactavatar {
  margin-bottom: 25px;
}
p {
  font-size: large;
}
</style>
