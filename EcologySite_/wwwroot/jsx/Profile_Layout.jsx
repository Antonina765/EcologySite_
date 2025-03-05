import React from 'react';
import axios from 'axios';
import PropTypes from 'prop-types';
import { AuthService } from '~/Services/AuthService'; 

const ProfileLayout = ({ userName, children }) => {
    const handleThemeToggle = (theme) => {
        document.body.setAttribute('data-theme', theme);
    };

    return (
        <div>
            <header className="head-page">
                <div className="logo">
                    <img src="~/images/Ecology/logotip.JPG" alt="logo" />
                </div>
                <div>Welcome {userName ? userName : 'Guest'}</div>
                <div className="reg-auth">
                    <ul className="navbar-nav flex-grow-1">
                        {!AuthService.isAuthenticated() ? (
                            <>
                                <li className="nav-item">
                                    <a className="nav-link text-dark" href="/Controllers/AuthController/Login">Login</a>
                                </li>
                                <li className="nav-item">
                                    <a className="nav-link text-dark" href="/Controllers/AuthController/Register">Registration</a>
                                </li>
                            </>
                        ) : (
                            <li className="nav-item">
                                <a className="nav-link text-dark" href="/Controllers/AuthController/Logout">Exit</a>
                            </li>
                        )}
                        {AuthService.isAdmin() && (
                            <li className="nav-item">
                                <a className="nav-link text-dark" href="/Admin/Users">Users</a>
                            </li>
                        )}
                    </ul>
                </div>
                <div>
                    <button className="theme-toggle" onClick={() => handleThemeToggle('light')}>Light Mode</button>
                    <button className="theme-toggle" onClick={() => handleThemeToggle('dark')}>Dark Mode</button>
                </div>
                <div className="return-home">
                    <ul>
                        <li><a className="return-home" href="/"></a></li>
                    </ul>
                </div>
            </header>
            <div className="user-content">
                {children}
            </div>
        </div>
    );
};

ProfileLayout.propTypes = {
    userName: PropTypes.string,
    children: PropTypes.node.isRequired,
};

export default ProfileLayout;
