import { Switch, Route } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import NavMenu from './components/NavMenu';
import  Login  from './components/Login';
import Entries from './components/Entries/Entries';
import Home from './components/Home';
import { useState, useEffect } from 'react';
import UserContext from './UserContext';

import 'bootstrap/dist/css/bootstrap.min.css'
import './custom.css'
import axios from 'axios';

export default function App() {

  const [sessionWorker, setSessionWorker] = useState(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const value = { sessionWorker, setSessionWorker, isLoggedIn, setIsLoggedIn };

  useEffect(() => {
    if(!isLoggedIn){
      setSessionWorker(null);
    }
  }, [isLoggedIn]);

  useEffect(() => {
    const getSessionUser = async () => {
      const response = await axios.get('api/workers/session');
      if(response.status === 200){
        setSessionWorker(response.data)
        setIsLoggedIn(true);
      }
    }
    getSessionUser();
  }, []);

  return (
    <UserContext.Provider value={value} >
      <NavMenu />
        <Container>
          <Switch>
              <Route exact path='/'>
                <Login />
              </Route>
              <Route path='/home'>
                <Home />
              </Route>
              <Route path='/entries'>
                <Entries />
              </Route>
          </Switch>
        </Container>
    </UserContext.Provider>
  );
  
}
