(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _alt = require('../alt');

var _alt2 = _interopRequireDefault(_alt);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var AddAccountActions = function () {
    function AddAccountActions() {
        _classCallCheck(this, AddAccountActions);

        this.generateActions('addAccountSuccess', 'addAccountFail', 'updateAccount', 'updatePassword', 'updateEmail', 'updateUsername', 'invalidAccount', 'invalidPassword', 'invalidEmail', 'invalidUsername');
    }

    _createClass(AddAccountActions, [{
        key: 'addAccount',
        value: function addAccount(account, password, email, username) {
            var _this = this;

            $.ajax({
                type: 'POST',
                url: '/api/accounts',
                data: { account: account, password: password, email: email, username: username }
            }).done(function (data) {
                _this.actions.addAccountSuccess(data.message);
            }).fail(function (jqXhr) {
                _this.actions.addAccountFail(jqXhr.responseJSON.message);
            });
        }
    }]);

    return AddAccountActions;
}();

exports.default = _alt2.default.createActions(AddAccountActions);

},{"../alt":4}],2:[function(require,module,exports){
"use strict";

},{}],3:[function(require,module,exports){
"use strict";

},{}],4:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _alt = require('alt');

var _alt2 = _interopRequireDefault(_alt);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = new _alt2.default();

},{"alt":"alt"}],5:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _AddAccountStore = require('../stores/AddAccountStore');

var _AddAccountStore2 = _interopRequireDefault(_AddAccountStore);

var _AddAccountActions = require('../actions/AddAccountActions');

var _AddAccountActions2 = _interopRequireDefault(_AddAccountActions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var AddAccount = function (_React$Component) {
    _inherits(AddAccount, _React$Component);

    function AddAccount(props) {
        _classCallCheck(this, AddAccount);

        var _this = _possibleConstructorReturn(this, Object.getPrototypeOf(AddAccount).call(this, props));

        _this.state = _AddAccountStore2.default.getState();
        _this.onChange = _this.onChange.bind(_this);
        return _this;
    }

    _createClass(AddAccount, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            _AddAccountStore2.default.listen(this.onChange);
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            _AddAccountStore2.default.unlisten(this.onChange);
        }
    }, {
        key: 'onChange',
        value: function onChange(state) {
            this.setState(state);
        }
    }, {
        key: 'handleSubmit',
        value: function handleSubmit(event) {
            event.preventDefault();

            var account = this.state.account.trim();
            var password = this.state.password.trim();
            var email = this.state.email.trim();
            var username = this.state.username.trim();

            if (!account) {
                _AddAccountActions2.default.onInvalidAccount();
                this.refs.accountTextField.getDOMNode().focus();
            }

            if (!password) {
                _AddAccountActions2.default.onInvalidPassword();
                this.refs.passwordTextField.getDOMNode().focus();
            }

            if (!email) {
                _AddAccountActions2.default.onInvalidEmail();
                this.refs.emailTextField.getDOMNode().focus();
            }

            if (!username) {
                _AddAccountActions2.default.onInvalidUsername();
                this.refs.usernameTextField.getDOMNode().focus();
            }

            if (account && password && email && username) {
                _AddAccountActions2.default.addAccount(account, password, email, username);
                this.refs.accountTextField.getDOMNode().focus();
            }
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'container' },
                _react2.default.createElement(
                    'div',
                    { className: 'row flipInX animated' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-sm-8' },
                        _react2.default.createElement(
                            'div',
                            { className: 'panel panel-default' },
                            _react2.default.createElement(
                                'div',
                                { className: 'panel-body' },
                                _react2.default.createElement(
                                    'form',
                                    { onSubmit: this.handleSubmit.bind(this) },
                                    _react2.default.createElement(
                                        'div',
                                        { className: 'form-group ' + this.state.accountValidationState },
                                        _react2.default.createElement(
                                            'label',
                                            { className: 'control-label' },
                                            'Account'
                                        ),
                                        _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'accountTextField', value: this.state.account,
                                            onChange: _AddAccountActions2.default.updateAccount, autoFocus: true }),
                                        _react2.default.createElement(
                                            'span',
                                            { className: 'help-block' },
                                            this.state.helpBlock
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'div',
                                        { className: 'form-group ' + this.state.passwordValidationState },
                                        _react2.default.createElement(
                                            'label',
                                            { className: 'control-label' },
                                            'Password'
                                        ),
                                        _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'passwordTextField', value: this.state.password,
                                            onChange: _AddAccountActions2.default.updatePassword, autoFocus: true }),
                                        _react2.default.createElement(
                                            'span',
                                            { className: 'help-block' },
                                            this.state.helpBlock
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'div',
                                        { className: 'form-group ' + this.state.emailValidationState },
                                        _react2.default.createElement(
                                            'label',
                                            { className: 'control-label' },
                                            'Email'
                                        ),
                                        _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'accountTextField', value: this.state.email,
                                            onChange: _AddAccountActions2.default.updateEmail, autoFocus: true }),
                                        _react2.default.createElement(
                                            'span',
                                            { className: 'help-block' },
                                            this.state.helpBlock
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'div',
                                        { className: 'form-group ' + this.state.usernamelValidationState },
                                        _react2.default.createElement(
                                            'label',
                                            { className: 'control-label' },
                                            'Username'
                                        ),
                                        _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'accountTextField', value: this.state.username,
                                            onChange: _AddAccountActions2.default.updateUsername, autoFocus: true }),
                                        _react2.default.createElement(
                                            'span',
                                            { className: 'help-block' },
                                            this.state.helpBlock
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'button',
                                        { type: 'submit', className: 'btn btn-primary' },
                                        'Submit'
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
    }]);

    return AddAccount;
}(_react2.default.Component);

exports.default = AddAccount;

},{"../actions/AddAccountActions":1,"../stores/AddAccountStore":13,"react":"react"}],6:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _Header = require('./Header');

var _Header2 = _interopRequireDefault(_Header);

var _Footer = require('./Footer');

var _Footer2 = _interopRequireDefault(_Footer);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var App = function (_React$Component) {
    _inherits(App, _React$Component);

    function App() {
        _classCallCheck(this, App);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(App).apply(this, arguments));
    }

    _createClass(App, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                null,
                _react2.default.createElement(_Header2.default, { history: this.props.history }),
                this.props.children,
                _react2.default.createElement(_Footer2.default, null)
            );
        }
    }]);

    return App;
}(_react2.default.Component);

exports.default = App;

},{"./Footer":7,"./Header":8,"react":"react"}],7:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

var _FooterStore = require('../stores/FooterStore');

var _FooterStore2 = _interopRequireDefault(_FooterStore);

var _FooterActions = require('../actions/FooterActions');

var _FooterActions2 = _interopRequireDefault(_FooterActions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Footer = function (_React$Component) {
    _inherits(Footer, _React$Component);

    function Footer(props) {
        _classCallCheck(this, Footer);

        var _this = _possibleConstructorReturn(this, Object.getPrototypeOf(Footer).call(this, props));

        {/*this.state = FooterStore.getState();*/}
        _this.onChange = _this.onChange.bind(_this);
        return _this;
    }

    _createClass(Footer, [{
        key: 'componentDidMount',
        value: function componentDidMount() {}
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            {/*FooterStore.unlisten(this.onChange);*/}
        }
    }, {
        key: 'onChange',
        value: function onChange(state) {
            this.setState(state);
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'footer',
                null,
                _react2.default.createElement(
                    'div',
                    { className: 'container' },
                    _react2.default.createElement(
                        'div',
                        { className: 'row' },
                        _react2.default.createElement(
                            'div',
                            { className: 'col-sm-5' },
                            _react2.default.createElement(
                                'h3',
                                { className: 'lead' },
                                _react2.default.createElement(
                                    'strong',
                                    null,
                                    'Password Manager'
                                )
                            ),
                            _react2.default.createElement(
                                'p',
                                null,
                                'View the source code behind this project on ',
                                _react2.default.createElement(
                                    'a',
                                    { href: 'https://github.com/DavidMGardner/FMEPasswordManager' },
                                    'GitHub.'
                                )
                            ),
                            _react2.default.createElement(
                                'p',
                                null,
                                '© 2016 Full Metal Engineering'
                            ),
                            _react2.default.createElement(
                                'h4',
                                null,
                                'Brutally Secure'
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Footer;
}(_react2.default.Component);

exports.default = Footer;

},{"../actions/FooterActions":2,"../stores/FooterStore":14,"react":"react","react-router":"react-router"}],8:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

var _HeaderStore = require('../stores/HeaderStore');

var _HeaderStore2 = _interopRequireDefault(_HeaderStore);

var _HeaderActions = require('../actions/HeaderActions');

var _HeaderActions2 = _interopRequireDefault(_HeaderActions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Header = function (_React$Component) {
    _inherits(Header, _React$Component);

    function Header(props) {
        _classCallCheck(this, Header);

        var _this = _possibleConstructorReturn(this, Object.getPrototypeOf(Header).call(this, props));

        {/*this.state = HeaderStore.getState();*/}
        _this.onChange = _this.onChange.bind(_this);
        return _this;
    }

    _createClass(Header, [{
        key: 'componentDidMount',
        value: function componentDidMount() {}
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            {/*HeaderStore.unlisten(this.onChange);*/}
        }
    }, {
        key: 'onChange',
        value: function onChange(state) {
            this.setState(state);
        }
    }, {
        key: 'handleSubmit',
        value: function handleSubmit(event) {
            event.preventDefault();
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'nav',
                { className: 'navbar navbar-default' },
                _react2.default.createElement(
                    'div',
                    { className: 'container' },
                    _react2.default.createElement(
                        'div',
                        { className: 'navbar-header' },
                        _react2.default.createElement(
                            'div',
                            { className: 'pull-left' },
                            _react2.default.createElement(
                                'h2',
                                null,
                                'Full Metal Engineering Password Manager'
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Header;
}(_react2.default.Component);

exports.default = Header;

},{"../actions/HeaderActions":3,"../stores/HeaderStore":15,"react":"react","react-router":"react-router"}],9:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Login = function (_React$Component) {
    _inherits(Login, _React$Component);

    function Login() {
        _classCallCheck(this, Login);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(Login).apply(this, arguments));
    }

    _createClass(Login, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                null,
                _react2.default.createElement(
                    'div',
                    { className: 'container' },
                    _react2.default.createElement(
                        'div',
                        { className: 'row' },
                        _react2.default.createElement(
                            'div',
                            { className: 'alert alert-info' },
                            'Landing Component'
                        ),
                        _react2.default.createElement(
                            _reactRouter.Link,
                            { to: '/table/' },
                            _react2.default.createElement(
                                'div',
                                { className: 'alert alert-warning' },
                                'Table Component'
                            )
                        )
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'container' },
                    _react2.default.createElement(
                        'div',
                        { className: 'row flipInX animated' },
                        _react2.default.createElement(
                            'div',
                            { className: 'col-sm-6' },
                            _react2.default.createElement(
                                'div',
                                { className: 'panel panel-default' },
                                _react2.default.createElement(
                                    'div',
                                    { className: 'panel-body' },
                                    _react2.default.createElement(
                                        'form',
                                        null,
                                        _react2.default.createElement(
                                            'div',
                                            { className: 'form-group ' },
                                            _react2.default.createElement(
                                                'label',
                                                { className: 'control-label' },
                                                'Email'
                                            ),
                                            _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'usernameTextField' })
                                        ),
                                        _react2.default.createElement(
                                            'div',
                                            { className: 'form-group ' },
                                            _react2.default.createElement(
                                                'label',
                                                { className: 'control-label' },
                                                'Password'
                                            ),
                                            _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'passwordTextField' })
                                        ),
                                        _react2.default.createElement(
                                            'button',
                                            { type: 'submit', className: 'btn btn-primary pull-right' },
                                            'Login'
                                        )
                                    )
                                )
                            )
                        ),
                        _react2.default.createElement(
                            'div',
                            { className: 'col-sm-6' },
                            _react2.default.createElement(
                                'div',
                                { className: 'panel panel-default' },
                                _react2.default.createElement(
                                    'div',
                                    { className: 'panel-body' },
                                    _react2.default.createElement(
                                        'form',
                                        null,
                                        _react2.default.createElement(
                                            'div',
                                            { className: 'form-group ' },
                                            _react2.default.createElement(
                                                'label',
                                                { className: 'control-label' },
                                                'Email'
                                            ),
                                            _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'usernameTextField' })
                                        ),
                                        _react2.default.createElement(
                                            'div',
                                            { className: 'form-group ' },
                                            _react2.default.createElement(
                                                'label',
                                                { className: 'control-label' },
                                                'Password'
                                            ),
                                            _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'passwordTextField' })
                                        ),
                                        _react2.default.createElement(
                                            'div',
                                            { className: 'form-group ' },
                                            _react2.default.createElement(
                                                'label',
                                                { className: 'control-label' },
                                                'Confirm Password'
                                            ),
                                            _react2.default.createElement('input', { type: 'text', className: 'form-control', ref: 'confirmPasswordTextField' })
                                        ),
                                        _react2.default.createElement(
                                            'button',
                                            { type: 'submit', className: 'btn btn-primary pull-right' },
                                            'Register'
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Login;
}(_react2.default.Component);

exports.default = Login;

},{"react":"react","react-router":"react-router"}],10:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var PasswordTable = function (_React$Component) {
    _inherits(PasswordTable, _React$Component);

    function PasswordTable(props) {
        _classCallCheck(this, PasswordTable);

        var _this = _possibleConstructorReturn(this, Object.getPrototypeOf(PasswordTable).call(this, props));

        _this.state = PasswordTableStore.getState();
        _this.onChange = _this.onChange.bind(_this);
        return _this;
    }

    _createClass(PasswordTable, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            PasswordTableStore.listen(this.onChange);
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            PassowordTableStore.unlisten(this.onChange);
        }
    }, {
        key: 'onChange',
        value: function onChange(state) {
            this.setState(state);
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'container' },
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'alert alert-info' },
                        'Table Component'
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'table-responsive' },
                        _react2.default.createElement(
                            'table',
                            { className: 'table table-hover' },
                            _react2.default.createElement(
                                'thead',
                                null,
                                _react2.default.createElement(
                                    'tr',
                                    { className: 'text-center' },
                                    _react2.default.createElement('td', null),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            'b',
                                            null,
                                            'Account'
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            'b',
                                            null,
                                            'Password'
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            'b',
                                            null,
                                            'Email'
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            'b',
                                            null,
                                            'Username'
                                        )
                                    ),
                                    _react2.default.createElement('td', null)
                                )
                            ),
                            _react2.default.createElement(
                                'tbody',
                                null,
                                _react2.default.createElement(
                                    'tr',
                                    { className: 'text-center' },
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-warning' },
                                                'Details'
                                            )
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'facebook.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'p@ssword1'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'email@gmail.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'user1'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-info' },
                                                'Edit'
                                            )
                                        )
                                    )
                                ),
                                _react2.default.createElement(
                                    'tr',
                                    { className: 'text-center' },
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-warning' },
                                                'Details'
                                            )
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'twitter.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'password1!'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'email.twitter@gmail.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'user2'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-info' },
                                                'Edit'
                                            )
                                        )
                                    )
                                ),
                                _react2.default.createElement(
                                    'tr',
                                    { className: 'text-center' },
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-warning' },
                                                'Details'
                                            )
                                        )
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'bank.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'p@ssword1'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'email.bank@gmail.com'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        'user3'
                                    ),
                                    _react2.default.createElement(
                                        'td',
                                        null,
                                        _react2.default.createElement(
                                            _reactRouter.Link,
                                            { to: '/table/' },
                                            _react2.default.createElement(
                                                'button',
                                                { type: 'submit', className: 'btn btn-info' },
                                                'Edit'
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    _react2.default.createElement(
                        _reactRouter.Link,
                        { to: '/add/' },
                        _react2.default.createElement(
                            'button',
                            { type: 'submit', className: 'btn btn-primary pull-right' },
                            'Add Account'
                        )
                    )
                )
            );
        }
    }]);

    return PasswordTable;
}(_react2.default.Component);

exports.default = PasswordTable;

},{"react":"react","react-router":"react-router"}],11:[function(require,module,exports){
'use strict';

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

var _reactDom = require('react-dom');

var _reactDom2 = _interopRequireDefault(_reactDom);

var _routes = require('./routes');

var _routes2 = _interopRequireDefault(_routes);

var _Header = require('./components/Header');

var _Header2 = _interopRequireDefault(_Header);

var _App = require('./components/App');

var _App2 = _interopRequireDefault(_App);

var _Login = require('./components/Login');

var _Login2 = _interopRequireDefault(_Login);

var _AddAccount = require('./components/AddAccount');

var _AddAccount2 = _interopRequireDefault(_AddAccount);

var _PasswordTable = require('./components/PasswordTable');

var _PasswordTable2 = _interopRequireDefault(_PasswordTable);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_reactDom2.default.render(_react2.default.createElement(
   _reactRouter.Router,
   { history: _reactRouter.browserHistory },
   _routes2.default
), document.getElementById('app'));

},{"./components/AddAccount":5,"./components/App":6,"./components/Header":8,"./components/Login":9,"./components/PasswordTable":10,"./routes":12,"react":"react","react-dom":"react-dom","react-router":"react-router"}],12:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactRouter = require('react-router');

var _App = require('./components/App');

var _App2 = _interopRequireDefault(_App);

var _Login = require('./components/Login');

var _Login2 = _interopRequireDefault(_Login);

var _AddAccount = require('./components/AddAccount');

var _AddAccount2 = _interopRequireDefault(_AddAccount);

var _PasswordTable = require('./components/PasswordTable');

var _PasswordTable2 = _interopRequireDefault(_PasswordTable);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _react2.default.createElement(
    _reactRouter.Route,
    { component: _App2.default },
    _react2.default.createElement(_reactRouter.Route, { path: '/', component: _Login2.default }),
    _react2.default.createElement(_reactRouter.Route, { path: '/add', component: _AddAccount2.default }),
    _react2.default.createElement(_reactRouter.Route, { path: '/table', component: _PasswordTable2.default })
);

},{"./components/AddAccount":5,"./components/App":6,"./components/Login":9,"./components/PasswordTable":10,"react":"react","react-router":"react-router"}],13:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _alt = require('../alt');

var _alt2 = _interopRequireDefault(_alt);

var _AddAccountActions = require('../actions/AddAccountActions');

var _AddAccountActions2 = _interopRequireDefault(_AddAccountActions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var AddAccountStore = function () {
    function AddAccountStore() {
        _classCallCheck(this, AddAccountStore);

        this.bindActions(_AddAccountActions2.default);
        this.account = '';
        this.password = '';
        this.email = '';
        this.username = '';
        this.helpBlock = '';
        this.accountValidationState = '';
        this.passwordValidationState = '';
        this.emailValidationState = '';
        this.usernameValidationState = '';
    }

    _createClass(AddAccountStore, [{
        key: 'onAddAccountSuccess',
        value: function onAddAccountSuccess(successMessage) {
            this.accountValidationState = 'has-success';
            this.helpBlock = successMessage;
        }
    }, {
        key: 'onAddAccountFail',
        value: function onAddAccountFail(errorMessage) {
            this.accountValidationState = 'has-error';
            this.helpBlock = errorMessage;
        }
    }, {
        key: 'onUpdateAccount',
        value: function onUpdateAccount(event) {
            this.account = event.target.value;
            this.accountValidationState = '';
            this.helpBlock = '';
        }
    }, {
        key: 'onUpdatePassword',
        value: function onUpdatePassword(event) {
            this.password = event.target.value;
            this.passwordValidationState = '';
            this.helpBlock = '';
        }
    }, {
        key: 'onUpdateEmail',
        value: function onUpdateEmail(event) {
            this.email = event.target.value;
            this.emailValidationState = '';
            this.helpBlock = '';
        }
    }, {
        key: 'onUpdateUsername',
        value: function onUpdateUsername(event) {
            this.username = event.target.value;
            this.usernameValidationState = '';
        }
    }, {
        key: 'onInvalidAccount',
        value: function onInvalidAccount() {
            this.accountValidationState = 'has-error';
            this.helpBlock = 'Please enter an account name.';
        }
    }, {
        key: 'onInvalidPassword',
        value: function onInvalidPassword() {
            this.passwordValidationState = 'has-error';
            this.helpBlock = 'Please enter a valid.';
        }
    }, {
        key: 'onInvalidEmail',
        value: function onInvalidEmail() {
            this.emailValidationState = 'has-error';
            this.helpBlock = 'Please enter an email';
        }
    }, {
        key: 'onInvalidUsername',
        value: function onInvalidUsername() {
            this.usernameValidationState = 'has-error';
            this.helpBlock = 'Please enter a username';
        }
    }]);

    return AddAccountStore;
}();

exports.default = _alt2.default.createStore(AddAccountStore);

},{"../actions/AddAccountActions":1,"../alt":4}],14:[function(require,module,exports){
"use strict";

},{}],15:[function(require,module,exports){
"use strict";

},{}]},{},[11])


//# sourceMappingURL=bundle.js.map
