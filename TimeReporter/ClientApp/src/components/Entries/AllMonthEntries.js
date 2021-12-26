import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import { useState, useEffect } from 'react';
import Moment from 'moment';


export default function AllMonthEntries({ selectedDate, report }){
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    
    const handleShow = () => setShow(true);

    useEffect(() => {
        const compare = ( a, b ) => {
            if ( a.date < b.date ){
              return -1;
            }
            if ( a.date > b.date ){
              return 1;
            }
            return 0;
        }
        
        if(report !== null && report.entries.length > 0){
            report.entries.sort( compare );
        }
    }, [report]);

    if(report !== null && report.entries.length > 0){
        return (
            <div className='col-sm-2'>
                <Button variant="primary" onClick={handleShow}>
                    Month Entries
                </Button>
    
                <Modal size='lg' show={show} onHide={handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>{Moment(selectedDate).format('MMMM YYYY')} Summary</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                    <table className='table table-striped' aria-labelledby="tabelLabel">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Date</th>
                                <th>Code</th>
                                <th>Subcode</th>
                                <th>Time</th>
                                <th>Description</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {report.entries.map((entry, i) =>
                            <tr key={entry.entryId}>
                                <td>{i+1}</td>
                                <td>{Moment(entry.date).format('YYYY-MM-DD')}</td>
                                <td>{entry.activity.code}</td>
                                <td>{entry.subactivity === null ? '-' : entry.subactivity.code}</td>
                                <td>{entry.time}</td>
                                <td>{entry.description === null ? '-' : entry.description}</td>                   
                            </tr>
                            )}
                        </tbody>
                    </table>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>
                            Close
                        </Button>
                    </Modal.Footer>
                </Modal>
            </div>
        );      
    }
    return(
        <div className="col-sm-2">
            <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">You don't have entries in this month</Tooltip>}>
                <span className="d-inline-block">
                    <Button variant="primary" disabled>
                        Month Entries
                    </Button>
                </span>
            </OverlayTrigger>
        </div>
    );    
}