import alt from '../alt';

class PasswordTableActions {
    constructor() {
        this.generateActions(
            'setTableSuccess',
            'setTableFailure'
        );
    }

    setTable() {
        $.ajax({url: '/Home/GetListOfPassword' })
          .done((data) => {
              this.actions.setTableSuccess(data.message);
              console.log(data);//for testing
          })
          .fail((jqXhr) => {
              this.actions.setTableFailure(jqXhr.responseJSON.message);
          });
    }
}

export default alt.createActions(PasswordTableActions);