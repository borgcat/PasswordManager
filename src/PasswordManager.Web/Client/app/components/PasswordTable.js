import React from 'react';
import {Link} from 'react-router';
import ajax from 'superagent';
import PasswordTableStore from '../stores/PasswordTableStore'
import PasswordTableActions from '../actions/PasswordTableActions';


class PasswordTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = PasswordTableStore.getState();
        this.onChange = this.onChange.bind(this);
    }
    
    componentWillMount() {
    ajax.get('/Home/GetListOfPassword')
        .end((error, response) => {
            if (!error && response) {
                this.setState({ passwords: response.body });
            } else {
                console.log('Avery is an error', error);
            }
        }
    );
}
    
    componentDidMount() {
        PasswordTableStore.listen(this.onChange);
    }

    componentWillUnmount() {
        PassowordTableStore.unlisten(this.onChange);
    }

    onChange(state){
        this.setState(state);
    }
    render() {
        return (
            <div className="container">
                {/* Banner */}
                <div className="row">
                    <div className='alert alert-info'>
                        Table Component
                    </div>
                </div>
                <div>{this.props.passwords}</div>
                
                {/*{/* table 
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
                                <tr className="text-center">
                                    <td>
                                        <Link to={'/table/'}>
                                            <button type='submit' className='btn btn-warning'>Details</button>
                                        </Link>
                                    </td>
                                    <td>facebook.com</td>
                                    <td>p@ssword1</td>
                                    <td>email@gmail.com</td>
                                    <td>user1</td>
                                    <td>
                                        <Link to={'/table/'}>
                                            <button type='submit' className='btn btn-info'>Edit</button>
                                        </Link>
                                    </td>
                                </tr>                                
                            </tbody>
                        </table>
                    </div>
                    <Link to={'/add/'}>
                        <button type='submit' className='btn btn-primary pull-right'>Add Account</button>
                    </Link>
                </div>*/}
            </div>
        );
    }
}

export default PasswordTable;