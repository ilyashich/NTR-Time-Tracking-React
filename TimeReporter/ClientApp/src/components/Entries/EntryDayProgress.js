import { ProgressBar, OverlayTrigger, Popover} from "react-bootstrap";

export default function EntryDayProgress( {totalSum} ){

    const progress = (totalSum / 500) * 100;

    const popover = (
        <Popover id="popover-basic">
          <Popover.Header as="h3">Daily Limit ({totalSum + "/500"})</Popover.Header>
          <Popover.Body>
            You can't submit more than 500 minutes daily
          </Popover.Body>
        </Popover>
    );

    if(totalSum === 0){
        return(
            <div></div>
        )
    }

    return(
        <div className="row">
            <div className="col-auto">
                <strong>Your daily progress</strong>
            </div>
            <div className="col-md-5">
                <OverlayTrigger placement="right" overlay={popover}>
                    <ProgressBar id="progress" animated now={progress} label={Math.round(progress) + "%"}/>
                </OverlayTrigger>
            </div>
        </div>
    )
}