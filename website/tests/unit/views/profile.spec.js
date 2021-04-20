import { mount } from '@vue/test-utils';
import Profile from '@/views/Profile.vue';
import { getContacts } from '@/utils/contactTools.js';

xdescribe('views/Profile.vue', () => {
  let wrapper;

  beforeAll(() => {
    wrapper = mount(Profile, {
      stubs: [
        'b-form-input',
        'b-button',
        'b-modal',
        'b-form-group',
        'b-avatar',
        'font-awesome-icon',
      ],
    });
  });

  /*comparing one by one because trying to compare all of the
   * props at once only results in an undefined result
   */
  it('props are passed in correctly', () => {
    expect(wrapper.vm.usersname).toBe('John Doe');
    expect(wrapper.vm.username).toBe('COOL_USER123');
    expect(wrapper.vm.title).toBe('');
    expect(wrapper.vm.inputname).toBe('');
    expect(wrapper.vm.nameState).toBe(null);
    expect(wrapper.vm.inputnumber).toBe('');
    expect(wrapper.vm.numberState).toBe(null);
    expect(wrapper.vm.saveindex).toBe(0);
    expect(wrapper.vm.contacts).toEqual(getContacts());
  });

  it('adding a contact adds and resets properly', async () => {
    //not showing
    expect(wrapper.vm.popupShow).toBe(false);

    //calls add method
    var addButton = wrapper.find('#add_button');
    await addButton.trigger('click');
    expect(wrapper.emitted('add'));

    //call method for the wrapper
    await wrapper.vm.add();

    //showing
    expect(wrapper.vm.popupShow).toBe(true);

    //variables as expected
    expect(wrapper.vm.inputname).toBe('');
    expect(wrapper.vm.inputnumber).toBe('');
    expect(wrapper.vm.title).toBe('Add Contact');

    //make an array to compare
    var newContact = { name: 'a', number: '123' };
    var newContacts = getContacts();
    newContacts.push(newContact);

    //change wrapper inputname and inputnumber
    wrapper.vm.inputname = 'a';
    wrapper.vm.inputnumber = '123';

    //test that it can be triggered
    var popup = wrapper.find('#popup');
    await popup.trigger('ok');
    expect(wrapper.emitted('handleSubmit'));

    //test that function adds it
    await wrapper.vm.handleSubmit();
    var current = wrapper.vm.contacts;
    expect(current).toEqual(newContacts);

    //not showing
    expect(wrapper.vm.popupShow).toBe(false);

    //delete added contact
    wrapper.vm.deleteContact(4);
  });

  it('editing a contact adds and resets properly', async () => {
    //not showing
    expect(wrapper.vm.popupShow).toBe(false);

    //call method for the wrapper
    //this is usually done by the child
    await wrapper.vm.edit(1);

    //showing
    expect(wrapper.vm.popupShow).toBe(true);

    //variables as expected
    expect(wrapper.vm.inputname).toBe('Harrison');
    expect(wrapper.vm.inputnumber).toBe('123-123-123');
    expect(wrapper.vm.title).toBe('Edit Contact');

    //make an array to compare
    var newContact = { name: 'a', number: '123' };
    var newContacts = getContacts();
    newContacts.splice(1, 1);
    newContacts.push(newContact);

    //change wrapper inputname and inputnumber
    wrapper.vm.inputname = 'a';
    wrapper.vm.inputnumber = '123';

    //test that it can be triggered
    var popup = wrapper.find('#popup');
    await popup.trigger('ok');
    expect(wrapper.emitted('handleSubmit'));

    await wrapper.vm.handleSubmit();
    var current = wrapper.vm.contacts;
    expect(current).toEqual(newContacts);

    //not showing
    expect(wrapper.vm.popupShow).toBe(false);

    //reset contacts
    wrapper.vm.contacts = getContacts();
  });

  it('deleting a contact deletes properly', async () => {
    //make an array to compare
    var newContacts = getContacts();
    newContacts.splice(0, 1);
    //test that function deletes it
    await wrapper.vm.deleteContact(0);
    var current = wrapper.vm.contacts;
    expect(current).toEqual(newContacts);
  });
});
