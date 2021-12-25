import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import { useState } from 'react';
import Moment from 'moment';


export default function Accepteds({ selectedDate, report }){
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    
    const handleShow = () => setShow(true);

    const accepteds = report !== null ? report.accepteds : [];

    const modalButton = accepteds.length > 0
        ? 
        <Button variant="primary" onClick={handleShow}>
            Month Summary
        </Button>
        :
        <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">You don't have accepted activities in this month</Tooltip>}>
            <span className="d-inline-block">
                <Button variant="primary" disabled>
                    Month Summary
                </Button>
            </span>
        </OverlayTrigger>

    return (
        <div className='col-sm-2'>
            {modalButton}

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>{Moment(selectedDate).format('MMMM YYYY')} Summary</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <table className='table table-striped' aria-labelledby="tabelLabel">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Code</th>
                                <th>Accepted Time</th>
                                <th>Manager</th>
                            </tr>
                        </thead>
                        <tbody>
                            {accepteds.map((accepted, i) =>
                            <tr key={accepted.acceptedId}>
                                <td>{i+1}</td>
                                <td>{accepted.activity.code}</td>
                                <td>{accepted.time}</td>
                                <td>{accepted.activity.worker.name}</td>                   
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