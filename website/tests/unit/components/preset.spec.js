import { shallowMount } from '@vue/test-utils';
import Preset from '@/components/Preset.vue';

describe('components/Preset.vue', () => {
  const propsData = {
    faIcon: 'faIcon',
    name: 'name',
    selected: false,
  };
  let wrapper;

  beforeAll(() => {
    wrapper = shallowMount(Preset, {
      propsData,
      stubs: ['font-awesome-icon'],
    });
  });

  it('props are passed in correctly', () => {
    expect(wrapper.props()).toEqual(propsData);
  });

  it('icon is replaced by text when hovering over circle', async () => {
    expect(wrapper.find('#preset-text').isVisible()).toBe(false);
    expect(wrapper.find('#preset-icon').isVisible()).toBe(true);

    await wrapper.find('#preset-circle').trigger('mouseover');
    expect(wrapper.find('#preset-text').isVisible()).toBe(true);
    expect(wrapper.find('#preset-icon').isVisible()).toBe(false);

    await wrapper.find('#preset-circle').trigger('mouseleave');
    expect(wrapper.find('#preset-text').isVisible()).toBe(false);
    expect(wrapper.find('#preset-icon').isVisible()).toBe(true);
  });

  it('lets parent know when circle is clicked', async () => {
    expect(wrapper.emitted().click).toBe(undefined);
    await wrapper.find('#preset-circle').trigger('click');
    expect(wrapper.emitted().click).toBeTruthy();
  });
});
