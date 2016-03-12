import React from 'react';
import { Router, Route, IndexRoute } from 'react-router';
import ReactDOM from 'react-dom';
import { browserHistory } from 'react-router'
import routes from './routes';
import Header from './components/Header';
import App from './components/App';
import Login from './components/Login';
import AddAccount from './components/AddAccount';
import PasswordTable from './components/PasswordTable';

ReactDOM.render(
    <Router history={browserHistory}>
       {routes}
    </Router>, document.getElementById('app')
);