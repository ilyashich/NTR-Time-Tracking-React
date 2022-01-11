import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link, useHistory } from 'react-router-dom';
import { useState, useContext } from 'react';
import './NavMenu.css';
import axios from 'axios';
import UserContext from '../UserContext';

export default function NavMenu() {

  const [collapsed, setCollapsed] = useState(true);

  const { sessionWorker, isLoggedIn, setIsLoggedIn } = useContext(UserContext);

  const history = useHistory();

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const handleLogout = () => {
    axios.get('api/workers/session/logout').then(response => {
      if(response.status === 200){
        setIsLoggedIn(false);
        history.replace("/");
      }
    })
  }

  let navbar = !isLoggedIn
    ? <div></div>
    : <Collapse className="d-sm-inline-flex justify-content-between" isOpen={!collapsed} navbar>
        <ul className="navbar-nav flex-grow-1">
          <NavItem>
            <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
          </NavItem>
          <NavItem>
              <NavLink tag={Link} className="text-dark" to="/entries">Entries</NavLink>
          </NavItem>
        </ul>
        <div className="col-auto">
            Logged as: {sessionWorker.name}
        </div>
        <div className="col-auto">
          <button id='login-button' className="btn btn-primary" onClick={handleLogout}>Logout</button>
        </div>
      </Collapse>;

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">TimeReporter</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          
            {navbar}
          
        </Container>
      </Navbar>
    </header>
  );
  
}
