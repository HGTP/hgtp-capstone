import { shallowMount } from '@vue/test-utils';
import GestureSettings from '@/views/GestureSettings.vue';

describe('views/GestureSettings.vue', () => {
  let wrapper;

  beforeAll(() => {
    wrapper = shallowMount(GestureSettings, {
      stubs: ['b-col', 'b-container', 'b-row'],
    });
  });

  describe('methods -> selectPreset', () => {
    it('updates the gestures to match the given preset', () => {
      expect(wrapper.vm.presetGestures).toEqual([]);
      wrapper.vm.selectPreset('Music');
      // TODO: Stub/mock the api to get a result fitting of a unit test.
    });
  });
});
