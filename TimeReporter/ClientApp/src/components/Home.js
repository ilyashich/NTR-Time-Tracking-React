import { Link } from "react-router-dom"

export default function Home(){
    return(
        <div>
            <div className="text-center">
                <p className="h3">Welcome to the Time Management System</p>
            </div>

            <div className="text-center">
                <Link to='/entries'>
                    <h4>Entries</h4>
                </Link>
            </div>
        </div>
    )
}