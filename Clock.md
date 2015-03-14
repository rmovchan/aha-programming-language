# Introduction #

This example demonstrates the declarative style of GUI programming with Aha! API. The _Timer_ extension is used.

The _Application_ is a GUI application with single visual control (a text label), displaying the _time_ component of the state, and an extension built as an instance of the _Timer_ object, calling the function _update_ at the default intervals of 1 second. The function simply puts the current time into the application state. The state is initially ( unknown: **nil** ), then changes to ( time: `<timestamp>` ).


# Code #

```
implement API/Application

use Time: Base/Time ` for Timestamp `
import Time(Types, Utils)

` Application state: `
type State: ( time: Timestamp | unknown: nil ) ` either time or unknown `
use App: API/AppTemplates<State>

use Controls: API/Controls<State>
import Controls(Widgets)
use Timer: API/Extensions/Timer<State>
import Timer(Everything)
use Formatting: Base/Formatting
import Formatting(Utils)

` Application instance definition: `
Application =
    get
        @App.GUIApp(display, ( unknown: nil )) ` initialize GUI application in 'unknown' state `
          .extensions([Timer(update, Second)^]) ` create a timer that calls update every second `
          ^ 
    where
        all
            display = Text ` display is a text label `
                .alignment({ state: State -> Alignment.center }) ` center in window `
                .style({ state: State -> TextStyle.attention }) ` enlarged font `
                .text({ state: State -> Format.time.short(state.time) }) ` display time part of timestamp `
                .visible({ state: State -> get nil where state.time? end }) ` display only when known `
                ^
            update = { time: Timestamp -> { state: State -> ( time: time ) } } ` update the state on timer event `
        end
    end
```