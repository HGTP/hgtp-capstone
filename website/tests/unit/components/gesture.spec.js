import { shallowMount } from '@vue/test-utils';
import Gesture from '@/components/Gesture.vue';

describe('components/Gesture.vue', () => {
  const propsData = {
    faIcon: 'faIcon',
    name: 'name',
    color: 'red',
  };
  let wrapper;

  beforeAll(() => {
    wrapper = shallowMount(Gesture, {
      propsData,
      stubs: ['font-awesome-icon', 'b-modal', 'b-button', 'b-form-select'],
    });
  });

  it('props are passed in correctly', () => {
    expect(wrapper.props()).toEqual(propsData);
  });
});
