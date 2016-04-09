import React from 'react';
import {Link} from 'react-router';
import $ from 'jquery';


// class PasswordTable extends React.Component {
//     constructor(props) {
//         super(props);
//         //this.state = PasswordTableStore.getState();
//         //this.onChange = this.onChange.bind(this);
//     }
    
// }         

var Password = React.createClass({
    render: function() {
        return (
            <tr className="text-center">
                <td>
                    <Link to={'/table/'}>
                        <button type='submit' className='btn btn-warning'>Details</button>
                    </Link>
                </td>
                <td>{this.props.CommonName}}</td>
                <td>{this.props.Url}</td>
                <td>{this.props.UserName}</td>
                <td>{this.props.Password}</td>
                <td>
                    <Link to={'/table/'}>
                        <button className='btn btn-info'>Edit</button>
                    </Link>
                </td>
            </tr>      
        );
    }
});

var PasswordList = React.createClass({
    render: function() {
        var passwordNode = this.props.data.map(function(password) { 
            return (
              <Password key={password.Id} />
            );
        });
        return (
            <div className="row">
                <div className="table-responsive">
                    <table className="table table-hover">
                        <thead>
                            <tr className="text-center">
                                <td></td>
                                <td><b>Account</b></td>
                                <td><b>Password</b></td>
                                <td><b>Email</b></td>
                                <td><b>Username</b></td>
                                <td></td>
                            </tr>
                        </thead>
                        <tbody>
                            {passwordNode}                          
                        </tbody>
                    </table>
                </div>
                    <button  className='btn btn-primary pull-right'>Add Account</button>
            </div>        
        );
    }
});

var PasswordForm = React.createClass({
    getInitialState: function() {
        return {CommonName: '', Url: '', UserName: '', Password: '' };
    },
    handleNameChange: function(e) {
        this.setState({CommonName: e.target.value});
    },
    handleUrlChange: function(e) {
        this.setState({Url: e.target.value});
    },
    handleUserNameChange: function(e) {
        this.setState({UserName: e.target.value});
    },
    handlePasswordChange: function(e) {
        this.setState({Password: e.target.value});
    },
    handleSubmit: function(e) {
        e.preventDefault();
        var CommonName = this.state.CommonName.trim();
        var Url = this.state.Url.trim();
        var UserName = this.state.UserName.trim();
        var Password = this.state.Password.trim();
        if (!CommonName || !Url || !UserName || !Password) {
        return;
        }
        this.props.onCommentSubmit({CommonName: CommonName, Url: Url, UserName: UserName, Password: Password});
        this.setState({CommonName: '', Url: '', UserName: '', Password: ''});
    },
    render: function() {
        return (
        <form className="passwordForm" onSubmit={this.handleSubmit}>
            <input
            type="text"
            placeholder="Common Name"
            value={this.state.CommonName}
            onChange={this.handleCommonNameChange}
            />
            <input
            type="text"
            placeholder="Url"
            value={this.state.Url}
            onChange={this.handleUrlChange}
            />
            <input
            type="text"
            placeholder="Username"
            value={this.state.UserName}
            onChange={this.handleUserNameChange}
            />
            <input
            type="text"
            placeholder="Password"
            value={this.state.Password}
            onChange={this.handlePasswordChange}
            />
            <input type="submit" value="Post" />
        </form>
        );
    }
});

var PasswordTable = React.createClass({
    loadPasswordsFromServer: function() {
        $.ajax({
        url: this.props.api,
        dataType: 'json',
        cache: false,
        success: function(data) {
            this.setState({data: data});
        }.bind(this),
        error: function(xhr, status, err) {
            console.error(this.props.url, status, err.toString());
        }.bind(this)
        });
    },
    handlePasswordSubmit: function(password) {
        $.ajax({
        url: this.props.api,
        dataType: 'json',
        type: 'POST',
        data: password,
        success: function(data) {
            this.setState({data: data});
        }.bind(this),
        error: function(xhr, status, err) {
            console.error(this.props.url, status, err.toString());
        }.bind(this)
        });
    },
    getInitialState: function() {
        return {data: []};
    },
    componentDidMount: function() {
        this.loadPasswordsFromServer();
        setInterval(this.loadPasswordsFromServer, this.props.pollInterval);
    },
    render: function() {
        return (
        <div className="passwordTable">
            <h1>Passwords</h1>
            <PasswordList data={this.state.data} />
            <PasswordForm onPasswordSubmit={this.handlePasswordSubmit} />
        </div>
        );
    }
});

export default PasswordTable;