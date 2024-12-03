original source: [https://adventofcode.com/2024/day/3](https://adventofcode.com/2024/day/3)
## --- Day 3: Mull It Over ---
"Our computers are having issues, so I have no idea if we have any Chief Historians in stock! You're welcome to check the warehouse, though," says the mildly flustered shopkeeper at the [North Pole Toboggan Rental Shop](/2020/day/2). The Historians head out to take a look.

The shopkeeper turns to you. "Any chance you can see why our computers are having issues again?"

The computer appears to be trying to run a program, but its memory (your puzzle input) is <em>corrupted</em>. All of the instructions have been jumbled up!

It seems like the goal of the program is just to <em>multiply some numbers</em>. It does that with instructions like <code>mul(X,Y)</code>, where <code>X</code> and <code>Y</code> are each 1-3 digit numbers. For instance, <code>mul(44,46)</code> multiplies <code>44</code> by <code>46</code> to get a result of <code>2024</code>. Similarly, <code>mul(123,4)</code> would multiply <code>123</code> by <code>4</code>.

However, because the program's memory has been corrupted, there are also many invalid characters that should be <em>ignored</em>, even if they look like part of a <code>mul</code> instruction. Sequences like <code>mul(4*</code>, <code>mul(6,9!</code>, <code>?(12,34)</code>, or <code>mul ( 2 , 4 )</code> do <em>nothing</em>.

For example, consider the following section of corrupted memory:

<pre>
<code>x<em>mul(2,4)</em>%&mul[3,7]!@^do_not_<em>mul(5,5)</em>+mul(32,64]then(<em>mul(11,8)mul(8,5)</em>)</code>
</pre>

Only the four highlighted sections are real <code>mul</code> instructions. Adding up the result of each instruction produces <code><em>161</em></code> (<code>2*4 + 5*5 + 11*8 + 8*5</code>).

Scan the corrupted memory for uncorrupted <code>mul</code> instructions. <em>What do you get if you add up all of the results of the multiplications?</em>


## --- Part Two ---
As you scan through the corrupted memory, you notice that some of the conditional statements are also still intact. If you handle some of the uncorrupted conditional statements in the program, you might be able to get an even more accurate result.

There are two new instructions you'll need to handle:


 - The <code>do()</code> instruction <em>enables</em> future <code>mul</code> instructions.
 - The <code>don't()</code> instruction <em>disables</em> future <code>mul</code> instructions.

Only the <em>most recent</em> <code>do()</code> or <code>don't()</code> instruction applies. At the beginning of the program, <code>mul</code> instructions are <em>enabled</em>.

For example:

<pre>
<code>x<em>mul(2,4)</em>&mul[3,7]!^<em>don't()</em>_mul(5,5)+mul(32,64](mul(11,8)un<em>do()</em>?<em>mul(8,5)</em>)</code>
</pre>

This corrupted memory is similar to the example from before, but this time the <code>mul(5,5)</code> and <code>mul(11,8)</code> instructions are <em>disabled</em> because there is a <code>don't()</code> instruction before them. The other <code>mul</code> instructions function normally, including the one at the end that gets re-<em>enabled</em> by a <code>do()</code> instruction.

This time, the sum of the results is <code><em>48</em></code> (<code>2*4 + 8*5</code>).

Handle the new instructions; <em>what do you get if you add up all of the results of just the enabled multiplications?</em>


