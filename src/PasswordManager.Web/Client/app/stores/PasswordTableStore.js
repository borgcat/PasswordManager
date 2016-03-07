import alt from '../alt';
import PasswordTableActions from '../actions/PasswordTableActions';

class PasswordTableStore {
    constructor() {
        this.bindActions(PasswordTableActions);
    }

    onAddCharacterSuccess(successMessage) {
        this.nameValidationState = 'has-success';
        this.helpBlock = successMessage;
    }

    onAddCharacterFail(errorMessage) {
        this.nameValidationState = 'has-error';
        this.helpBlock = errorMessage;
    }
}

export default alt.createStore(PasswordTableStore);