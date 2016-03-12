import alt from '../alt';
import PasswordTableActions from '../actions/PasswordTableActions';

class PasswordTableStore {
    constructor() {
        this.bindActions(PasswordTableActions);
        this.table = {};
    }

    onSetTableSuccess(data) {
        this.table = data;
    }

    onSetTableFailure(jqXhr) {
        // Handle multiple response formats, fallback to HTTP status code number.
        toastr.error(jqXhr.responseJSON && jqXhr.responseJSON.message || jqXhr.responseText || jqXhr.statusText);
    }
}

export default alt.createStore(PasswordTableStore);