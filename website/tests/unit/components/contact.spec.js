import { shallowMount } from '@vue/test-utils';
import Contact from '@/components/Contact.vue';

describe('components/Contact.vue', () => {
  const propsData = {
    name: 'user_name',
    number: '123',
  };
  let wrapper;

  beforeAll(() => {
    wrapper = shallowMount(Contact, {
      propsData,
      stubs: ['font-awesome-icon', 'b-avatar'],
    });
  });

  it('props are passed in correctly', () => {
    expect(wrapper.props()).toEqual(propsData);
  });

  it('parent alerted to delete icon being pressed', async () => {
    await wrapper.find('#trashicon').trigger('click');
    expect(wrapper.emitted()).toBeTruthy();
  });

  it('parent alerted to edit icon being pressed', async () => {
    await wrapper.find('#editicon').trigger('click');
    expect(wrapper.emitted()).toBeTruthy();
  });
});
