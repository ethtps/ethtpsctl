#!/usr/bin/perl
use strict;
use warnings;

my $filename = $ARGV[0];
my $definitions_file = $ARGV[1];

open my $fh, '<', $definitions_file or die "Could not open '$definitions_file' $!";
my $content = do { local $/; <$fh> };
close $fh;

my %replacements;
while ($content =~ /replace\s*{(.*?)}\s*with\s*{(.*?)}/gs) {
    $replacements{$1} = $2;
}

my @regex_replacements;
while ($content =~ /regexreplace\s*{(.*?)}\s*with\s*{(.*?)}/gs) {
    push @regex_replacements, [$1, $2];
}

open $fh, '<', $filename or die "Could not open '$filename' $!";
my $file_content = do { local $/; <$fh> };
close $fh;

my $previous_content;
do {
    $previous_content = $file_content;

    foreach my $key (keys %replacements) {
        my $value = $replacements{$key};
        $file_content =~ s/\Q$key\E/$value/gs;
    }

foreach my $pair (@regex_replacements) {
    my ($regex, $replacement) = @$pair;
    my $compiled_regex = qr/$regex/s;  # Compile the regex, enabling the /s modifier
    $replacement =~ s/\n/\\n/g;  # Escape newline characters
    $replacement = qq("$replacement");  # Wrap the replacement string in double quotes
    $file_content =~ s/$compiled_regex/$replacement/ee;
}

} while ($file_content ne $previous_content);  # Continue as long as the content is still changing

open $fh, '>', $filename or die "Could not open '$filename' $!";
print $fh $file_content;
close $fh;
