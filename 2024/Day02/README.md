original source: [https://adventofcode.com/2024/day/2](https://adventofcode.com/2024/day/2)
## --- Day 2: Red-Nosed Reports ---
Fortunately, the first location The Historians want to search isn't a long walk from the Chief Historian's office.

While the [Red-Nosed Reindeer nuclear fusion/fission plant](/2015/day/19) appears to contain no sign of the Chief Historian, the engineers there run up to you as soon as they see you. Apparently, they <em>still</em> talk about the time Rudolph was saved through molecular synthesis from a single electron.

They're quick to add that - since you're already here - they'd really appreciate your help analyzing some unusual data from the Red-Nosed reactor. You turn to check if The Historians are waiting for you, but they seem to have already divided into groups that are currently searching every corner of the facility. You offer to help with the unusual data.

The unusual data (your puzzle input) consists of many <em>reports</em>, one report per line. Each report is a list of numbers called <em>levels</em> that are separated by spaces. For example:

<pre>
<code>7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
</code>
</pre>

This example data contains six reports each containing five levels.

The engineers are trying to figure out which reports are <em>safe</em>. The Red-Nosed reactor safety systems can only tolerate levels that are either gradually increasing or gradually decreasing. So, a report only counts as safe if both of the following are true:


 - The levels are either <em>all increasing</em> or <em>all decreasing</em>.
 - Any two adjacent levels differ by <em>at least one</em> and <em>at most three</em>.

In the example above, the reports can be found safe or unsafe by checking those rules:


 - <code>7 6 4 2 1</code>: <em>Safe</em> because the levels are all decreasing by 1 or 2.
 - <code>1 2 7 8 9</code>: <em>Unsafe</em> because <code>2 7</code> is an increase of 5.
 - <code>9 7 6 2 1</code>: <em>Unsafe</em> because <code>6 2</code> is a decrease of 4.
 - <code>1 3 2 4 5</code>: <em>Unsafe</em> because <code>1 3</code> is increasing but <code>3 2</code> is decreasing.
 - <code>8 6 4 4 1</code>: <em>Unsafe</em> because <code>4 4</code> is neither an increase or a decrease.
 - <code>1 3 6 7 9</code>: <em>Safe</em> because the levels are all increasing by 1, 2, or 3.

So, in this example, <code><em>2</em></code> reports are <em>safe</em>.

Analyze the unusual data from the engineers. <em>How many reports are safe?</em>


## --- Part Two ---
The engineers are surprised by the low number of safe reports until they realize they forgot to tell you about the Problem Dampener.

The Problem Dampener is a reactor-mounted module that lets the reactor safety systems <em>tolerate a single bad level</em> in what would otherwise be a safe report. It's like the bad level never happened!

Now, the same rules apply as before, except if removing a single level from an unsafe report would make it safe, the report instead counts as safe.

More of the above example's reports are now safe:


 - <code>7 6 4 2 1</code>: <em>Safe</em> without removing any level.
 - <code>1 2 7 8 9</code>: <em>Unsafe</em> regardless of which level is removed.
 - <code>9 7 6 2 1</code>: <em>Unsafe</em> regardless of which level is removed.
 - <code>1 <em>3</em> 2 4 5</code>: <em>Safe</em> by removing the second level, <code>3</code>.
 - <code>8 6 <em>4</em> 4 1</code>: <em>Safe</em> by removing the third level, <code>4</code>.
 - <code>1 3 6 7 9</code>: <em>Safe</em> without removing any level.

Thanks to the Problem Dampener, <code><em>4</em></code> reports are actually <em>safe</em>!

Update your analysis by handling situations where the Problem Dampener can remove a single level from unsafe reports. <em>How many reports are now safe?</em>


