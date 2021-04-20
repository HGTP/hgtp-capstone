<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div id="feature">
    <b-row align-v="center">
      <b-col id="buttons">
        <b-button
          v-bind:class="{
            'feature-button': true,
            'active-vote': true,
            'float-right': true,
            'voted-for': votedForFeature,
          }"
          @click.stop="upvote()"
        >
          <font-awesome-icon icon="thumbs-up" size="1x" />
          <b-badge class="badge">{{ votesCount }}</b-badge>
        </b-button>
      </b-col>
      <b-col cols="11">
        <p id="description" class="align-middle">{{ description }}</p>
        <ul id="tags">
          <li v-for="tag in tags" :key="tag">
            <b-badge variant="info">{{ tag }}</b-badge>
          </li>
        </ul>
      </b-col>

      <b-form-select
        id="status-select"
        v-if="isAdmin"
        v-on:change="changeStatus"
        v-model="featureStatus"
        :options="statusOptions"
      ></b-form-select>
      <font-awesome-icon
        @click="deleteFeature()"
        v-if="isAdmin"
        id="delete-icon"
        icon="trash"
        size="2x"
      />
    </b-row>
  </div>
</template>

<script>
import {
  deleteFeature,
  removeVoteForFeature,
  updateFeatureStatus,
  voteForFeature,
} from '../utils/featureTools';

export default {
  computed: {
    bvModalId() {
      return `feature-bv-modal-${this.id}`;
    },
  },
  data() {
    return {
      featureStatus: this.status,
      statusOptions: ['unplanned', 'planned', 'completed'],
      votesCount: this.votes,
      votedForFeature: this.userVotedForThisFeature,
    };
  },
  methods: {
    async changeStatus() {
      await updateFeatureStatus(this.id, this.featureStatus);
      this.$emit('remove-feature-from-list', this.id);
    },
    async deleteFeature() {
      await deleteFeature(this.id);
      this.$emit('remove-feature-from-list', this.id);
    },
    async upvote() {
      if (this.votedForFeature) {
        await removeVoteForFeature(this.id);
        this.votedForFeature = false;
        this.votesCount--;
      } else {
        await voteForFeature(this.id);
        this.votedForFeature = true;
        this.votesCount++;
      }
    },
  },
  props: {
    description: String,
    id: Number,
    isAdmin: Boolean,
    status: String,
    tags: Array,
    userVotedForThisFeature: Boolean,
    votes: Number,
  },
};
</script>

<style scoped>
.badge {
  margin-left: 6px;
}
#buttons {
  border-right: 1px solid lightgray;
}
.card-title {
  font-size: 1.3em;
}
.card {
  margin: 6px 0 6px 0;
  width: 100%;
}
#delete-icon {
  position: absolute;
  right: 38px;
  color: #dc3545;
  cursor: pointer;
}
#delete-icon:hover {
  color: #b52a37;
}
#description {
  margin: 0 180px 0 0;
}
#feature {
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 12px;
  width: 100%;
  margin-bottom: 6px;
  min-height: 80px;
}
.feature-button {
  margin-right: 6px;
}
h3 {
  text-align: center;
}
#status-select {
  position: absolute;
  width: 120px;
  right: 80px;
}
#tags {
  list-style-type: none;
  padding: 0;
}
#tags .badge {
  margin: 0 4px 0 0;
}
#tags li {
  display: inline-block;
}
.voted-for {
  background-color: #007bff;
}
</style>
