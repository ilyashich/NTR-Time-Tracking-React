import React from 'react';

const UserContext = React.createContext({
    sessionWorker: null,
    setSessionWorker: () => {}, 
    isLoggedIn: false,
    setIsLoggedIn: () => {}
});
export default UserContext;