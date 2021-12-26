import React, { useState, useEffect } from "react";
import axios from "axios";
import Moment from "moment";
import FormData from 'form-data';
import AddEntry from "./AddEntry";
import EditEntry from "./EditEntry";
import DeleteEntry from "./DeleteEntry";
import EntryDayProgress from "./EntryDayProgress";
import Accepteds from "./Accepteds";
import { Alert } from "react-bootstrap";

export default function Entries(){

    const [entries, setEntries] = useState([]);
    const [activities, setActivities] = useState([]);
    const [report, setReport] = useState(null);
    const [date, setDate] = useState(Moment().format('YYYY-MM-DD'));

    const [reload, setReload] = useState(true);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    

    const month = date.split("-")[1];
    const year = date.split("-")[0];

    let totalSum = 0;


    useEffect(() => {
        const populateActivitiesData = async () => {
            const response = await axios.get('api/activities');
            setActivities(response.data);
        }
        populateActivitiesData();
    }, []);
    
    useEffect(() => {
        const getReport = async () => {
            const form_data = new FormData();
            form_data.append('date', year + "-" + month + "-01");
            const response = await axios.post('api/reports/worker', form_data);
            if(response.status === 204){
                setReport(null);
            }
            else{
                setReport(response.data);
            }
            setLoading(false);
        }
        getReport();
    }, [month, year, reload]);

    useEffect(() => {
        const getEntries = () => {
            const dayEntries = [];
            if(report !== null){
                for(let i = 0; i < report.entries.length; i++){
                    if(Moment(report.entries[i].date).format('YYYY-MM-DD') === date){
                        dayEntries.push(report.entries[i]);
                    }
                }
            }
            setEntries(dayEntries);
        }

        getEntries();
    }, [date, report]);

    const handleDateChange = (e) => {
        setDate(e.target.value);
        e.preventDefault();
    }

    const calculateTotal = (entries) => {
        for(let i = 0; i < entries.length; i++){
            totalSum += entries[i].time;
        }
    }

    const renderEntriesTable = (entries) => {
        if(entries.length === 0){
            return( 
                <div id="no-day-entries" className="text-center">
                    <h3>No entries for this day.</h3>
                </div>
            );
        }

        calculateTotal(entries);

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Code</th>
                    <th>Subcode</th>
                    <th>Time</th>
                    <th>Description</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {entries.map((entry, i) =>
                <tr key={entry.entryId}>
                    <td>{i+1}</td>
                    <td>{entry.activity.code}</td>
                    <td>{entry.subactivity === null ? '-' : entry.subactivity.code}</td>
                    <td>{entry.time}</td>
                    <td>{entry.description === null ? '-' : entry.description}</td>
                    <td className="col-auto">
                        <div className="row">
                            <EditEntry activities={activities} 
                                       entry={entry} 
                                       reload={reload} 
                                       setReload={setReload} 
                                       setLoading={setLoading} 
                                       totalSum={totalSum}
                                       isFrozen={report.frozen}
                                       setError={setError} />
                        
                            <DeleteEntry entryId={entry.entryId} 
                                         setLoading={setLoading} 
                                         reload={reload} 
                                         setReload={setReload}
                                         isFrozen={report.frozen}
                                         setError={setError} />
                        </div>
                    </td>                    
                </tr>
                )}
                <tr className="th">
                    <th colSpan="3">Total</th>
                    <th colSpan="3">{totalSum}</th>
                </tr>
            </tbody>
            </table>
        );
    }

    const renderPlaceholder = () => {

        let body = [];
        for(let i = 0; i < 5; ++i){
            body.push(
                <tr key={i}>
                    <td>
                        <span className="placeholder col-4"></span>
                    </td>
                    <td>
                        <span className="placeholder col-4"></span>
                    </td>
                    <td>
                        <span className="placeholder col-4"></span>
                    </td>
                    <td>
                        <span className="placeholder col-4"></span>
                    </td>
                </tr>
            )
        }
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead className="placeholder-glow">
                <tr>
                    <th>
                        <span className="placeholder col-4"></span>
                    </th>
                    <th>
                        <span className="placeholder col-4"></span>
                    </th>
                    <th>
                        <span className="placeholder col-4"></span>
                    </th>
                    <th>
                        <span className="placeholder col-4"></span>
                    </th>
                </tr>
            </thead>
            <tbody className="placeholder-glow">
                {body}
            </tbody>
            </table>
        );
    }

    //console.log(report);

    let contents = loading
        ? renderPlaceholder()
        : renderEntriesTable(entries);

    return(
        <div>
            { error && 
                <Alert variant="danger" onClose={() => setError('')} dismissible>
                    {error}
                </Alert>
            }
            <div className="mb-3 row">
                <label htmlFor="date" className="col-auto">
                    <strong>Select Date</strong>
                </label>
                <div className="col-md-4">
                    <input type='date' defaultValue={Moment(date).format('YYYY-MM-DD')} onChange={handleDateChange}/>
                </div>
                <div className="col">
                    <EntryDayProgress totalSum={totalSum} />
                </div>
            </div>
            <div className="row">
                <AddEntry activities={activities} 
                          selectedDate={Moment(date).format('YYYY-MM-DD')} 
                          reload={reload} 
                          setReload={setReload} 
                          setLoading={setLoading} 
                          totalSum={totalSum}
                          report={report}
                          setError={setError} />
                <Accepteds selectedDate={Moment(date).format('YYYY-MM-DD')} report={report} />
            </div>
            {contents}
        </div>
    );

}